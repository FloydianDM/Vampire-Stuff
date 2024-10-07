using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonobehaviour<PoolManager>
{
    [SerializeField] private List<Pool> _poolList;

    private Transform _objectPoolTransform;
    private Dictionary<int, Queue<Component>> _poolDictionary = new Dictionary<int, Queue<Component>>();

    [Serializable]
    public struct Pool
    {   
        public int PoolSize;
        public GameObject Prefab;
        public TypeEnum Type;
    }

    private void OnEnable()
    {
        _objectPoolTransform = gameObject.transform;

        for (int i = 0; i < _poolList.Count; i++)
        {
            CreatePool(_poolList[i]);
        }
    }

    private void CreatePool(Pool pool)
    {
        GameObject prefab = pool.Prefab;
        int poolSize = pool.PoolSize;
        TypeEnum typeEnum = pool.Type;

        int poolKey = prefab.GetInstanceID();

        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        parentGameObject.transform.SetParent(_objectPoolTransform);

        if (!_poolDictionary.ContainsKey(poolKey))
        {
            _poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform);

                newObject.SetActive(false);

                Type comType = GetTypeFromTypeEnum(typeEnum);

                Component newObjectComponent = newObject.GetComponent(comType);

                _poolDictionary[poolKey].Enqueue(newObjectComponent);
            }
        }
    }

    public Component ReuseComponent(GameObject prefab, Vector2 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (_poolDictionary.ContainsKey(poolKey))
        {
            Component componentToUse = GetComponentFromPool(poolKey);
            ResetObject(position, rotation, componentToUse, prefab);

            return componentToUse;
        }
        else
        {
            return null;
        }
    }

    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToUse = _poolDictionary[poolKey].Dequeue();
        _poolDictionary[poolKey].Enqueue(componentToUse);

        if (componentToUse.gameObject.activeSelf)
        {
            componentToUse.gameObject.SetActive(true);
        }

        return componentToUse;
    }

    private void ResetObject(Vector2 position, Quaternion rotation, Component componentToUse, GameObject prefab)
    {
        componentToUse.transform.position = position;
        componentToUse.transform.rotation = rotation;
        componentToUse.transform.localScale = prefab.transform.localScale;
    }

    private Type GetTypeFromTypeEnum(TypeEnum typeEnum)
    {
        Type type = null;

        switch (typeEnum)
        {
            case TypeEnum.Fireable:
                type = typeof(IFireable);
                break;
            case TypeEnum.Enemy:
                type = typeof(Enemy);
                break;
            case TypeEnum.EnemyDeathEffect:
                type = typeof(EnemyDeathEffect);
                break;
            case TypeEnum.Usable:
                type = typeof(IUsable);
                break;
        }

        return type;
    }

}