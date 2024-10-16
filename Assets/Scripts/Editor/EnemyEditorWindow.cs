using UnityEngine;
using UnityEditor;
using System;

// editor for creating enemy details SOs
public class EnemyEditorWindow : EditorWindow
{
    #region Prefabs
    private GameObject _basePrefab;
    private GameObject _tempPrefabVariant;
    private GameObject _newPrefab;
    private string _newPrefabVariantName;
    private GameObject _enemySpawnerPrefab;
    private EnemySpawner _enemySpawner;
    private GameObject _poolManagerPrefab;
    private PoolManager _poolManager;
    private SerializedObject _serializedEnemySpawnerObject;
    private SerializedProperty _enemyPrefabListProp;
    private SerializedObject _serializedPoolManagerObject;
    private SerializedProperty _poolListProp;
    #endregion

    #region Scriptable Objects
    private string _enemyType;
    private readonly string _assetPath = "Assets/ScriptableObjects/Enemies/";
    private EnemyDetailsSO _newEnemyDetails;
    private EnemyDetailsSO _selectedEnemyDetails;
    private SerializedObject _serializedScriptableObject;
    private SerializedProperty _typeProp;
    private SerializedProperty _spriteProp;
    private SerializedProperty _spriteColorProp;
    private SerializedProperty _speedMinProp;
    private SerializedProperty _speedMaxProp;
    private SerializedProperty _dodgeThrustProp;
    private SerializedProperty _healthMinProp;
    private SerializedProperty _healthMaxProp;
    private SerializedProperty _damageProp;
    private SerializedProperty _contactDamageMinProp;
    private SerializedProperty _contactDamageMaxProp;
    private SerializedProperty _experienceDropProp;
    private SerializedProperty _awarenessProp;
    private SerializedProperty _agilityProp;
    private SerializedProperty _enemyDeathEffect;
    #endregion

    private Vector2 _scrollPosition = Vector2.zero;

    [MenuItem("Tools/Enemy Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<EnemyEditorWindow>();
        window.titleContent = new GUIContent("Enemy Creator");
        window.Show();
    }

    private void OnGUI()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, true, true);

        GUILayout.Space(20);

        // create enemy section

        GUILayout.Label("Create An Enemy", EditorStyles.boldLabel);

        _enemyType = EditorGUILayout.TextField("Enemy Type", _enemyType);

        if (_basePrefab == null)
        {
            _basePrefab = Resources.Load<GameObject>("Enemy");
        }

        if (_enemySpawnerPrefab == null)
        {
            _enemySpawnerPrefab = Resources.Load<GameObject>("EnemySpawner");
        }

        if (_poolManagerPrefab == null)
        {
            _poolManagerPrefab = Resources.Load<GameObject>("PoolManager");
        }

        if (GUILayout.Button("Create New Enemy"))
        {
            if (_basePrefab == null || _enemySpawnerPrefab == null)
            {
                GUIContent notificationWindow = new GUIContent("There is no base prefab and/or enemy spawner prefab in resources!");
                ShowNotification(notificationWindow);
            }
            else
            {
                CreateScriptableObject();
            }
        }

        if (_newEnemyDetails != null)
        {
            GUILayout.Label("Initialize Enemy Details", EditorStyles.boldLabel);

            InitializeScriptableObject();

            if (GUILayout.Button("Save Enemy Details Scriptable Object"))
            {
                SaveScriptableObject();
                CreatePrefabVariant();
            }
        }
        
        if (_newPrefab != null && _enemySpawner == null)
        {
            _enemySpawner = AddEnemyToEnemySpawner(_newPrefab);
        }

        if (_enemySpawner != null)
        {
            EditEnemySpawner(_enemySpawner);

            if (GUILayout.Button("Save Enemy Spawner"))
            {
                SaveEnemySpawner();
            }
        }

        if (_newPrefab != null && _poolManager == null)
        {
            _poolManager = AddEnemyToPoolManager(_newPrefab);
        }

        if (_poolManager != null)
        {
            EditPoolManager(_poolManager);

            if (GUILayout.Button("Save Pool Manager"))
            {
                SavePoolManager();
            }
        }
    
        GUILayout.Space(20);

        // edit enemy section

        GUILayout.Label("Edit Enemy Details Scriptable Object", EditorStyles.boldLabel);

        _selectedEnemyDetails = (EnemyDetailsSO)EditorGUILayout.ObjectField(
            "Select Scriptable Object", _selectedEnemyDetails, typeof(EnemyDetailsSO), false);

        if (_selectedEnemyDetails != null)
        {
            EditScriptableObject();

            if (GUILayout.Button("Save Enemy Details Scriptable Object"))
            {
                SaveEditedScriptableObject();
            }
        }

        GUILayout.EndScrollView();
    }

    private void CreateScriptableObject()
    {
        _newEnemyDetails = CreateInstance<EnemyDetailsSO>();
    }

    private void InitializeScriptableObject()
    {
        _newEnemyDetails.Type = _enemyType;
        _newEnemyDetails.Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _newEnemyDetails.Sprite, typeof(Sprite), false);
        _newEnemyDetails.SpriteColor = EditorGUILayout.ColorField("Sprite Color", _newEnemyDetails.SpriteColor);
        _newEnemyDetails.SpeedMin = EditorGUILayout.FloatField("SpeedMin", _newEnemyDetails.SpeedMin);
        _newEnemyDetails.SpeedMax = EditorGUILayout.FloatField("SpeedMax", _newEnemyDetails.SpeedMax);
        _newEnemyDetails.DodgeThrust = EditorGUILayout.FloatField("DodgeThrust", _newEnemyDetails.DodgeThrust);
        _newEnemyDetails.HealthMin = EditorGUILayout.IntField("HealthMin", _newEnemyDetails.HealthMin);
        _newEnemyDetails.HealthMax = EditorGUILayout.IntField("HealthMax", _newEnemyDetails.HealthMax);
        _newEnemyDetails.Damage = EditorGUILayout.FloatField("Damage", _newEnemyDetails.Damage);
        _newEnemyDetails.ContactDamageMin = EditorGUILayout.IntField("ContactDamageMin", _newEnemyDetails.ContactDamageMin);
        _newEnemyDetails.ContactDamageMax = EditorGUILayout.IntField("ContactDamageMin", _newEnemyDetails.ContactDamageMax);
        _newEnemyDetails.ExperienceDrop = EditorGUILayout.IntSlider("ExperienceDrop", _newEnemyDetails.ExperienceDrop, 1, 20);
        _newEnemyDetails.Awareness = EditorGUILayout.Slider("Awareness", _newEnemyDetails.Awareness, 0, 2);
        _newEnemyDetails.Agility = EditorGUILayout.Slider("Agility", _newEnemyDetails.Agility, 0, 20);
        _newEnemyDetails.EnemyDeathEffect = (EnemyDeathEffectSO)EditorGUILayout.ObjectField(
            "EnemyDeathEffect", _newEnemyDetails.EnemyDeathEffect, typeof(EnemyDeathEffectSO), false);
    }

    private void EditScriptableObject()
    {
        if (_serializedScriptableObject == null || _serializedScriptableObject.targetObject != _selectedEnemyDetails)
        {
            _newEnemyDetails = null; // to clear the create section
            _enemyType = null; 
            _basePrefab = null; 

            _serializedScriptableObject = new SerializedObject(_selectedEnemyDetails);

            // get values from SO

            _typeProp = _serializedScriptableObject.FindProperty("Type");
            _spriteProp = _serializedScriptableObject.FindProperty("Sprite");
            _spriteColorProp = _serializedScriptableObject.FindProperty("SpriteColor");
            _speedMinProp = _serializedScriptableObject.FindProperty("SpeedMin");
            _speedMaxProp = _serializedScriptableObject.FindProperty("SpeedMax");
            _dodgeThrustProp = _serializedScriptableObject.FindProperty("DodgeThrust");
            _healthMinProp = _serializedScriptableObject.FindProperty("HealthMin");
            _healthMaxProp = _serializedScriptableObject.FindProperty("HealthMax");
            _damageProp = _serializedScriptableObject.FindProperty("Damage");
            _contactDamageMinProp = _serializedScriptableObject.FindProperty("ContactDamageMin");
            _contactDamageMaxProp = _serializedScriptableObject.FindProperty("ContactDamageMax");
            _experienceDropProp = _serializedScriptableObject.FindProperty("ExperienceDrop");
            _awarenessProp = _serializedScriptableObject.FindProperty("Awareness");
            _agilityProp = _serializedScriptableObject.FindProperty("Agility");
            _enemyDeathEffect = _serializedScriptableObject.FindProperty("EnemyDeathEffect");
        }

        _serializedScriptableObject.Update();

        EditorGUILayout.PropertyField(_typeProp);
        EditorGUILayout.PropertyField(_spriteProp);
        EditorGUILayout.PropertyField(_spriteColorProp);
        EditorGUILayout.PropertyField(_speedMinProp);
        EditorGUILayout.PropertyField(_speedMaxProp);
        EditorGUILayout.PropertyField(_dodgeThrustProp);
        EditorGUILayout.PropertyField(_healthMinProp);
        EditorGUILayout.PropertyField(_healthMaxProp);
        EditorGUILayout.PropertyField(_damageProp);
        EditorGUILayout.PropertyField(_contactDamageMinProp);
        EditorGUILayout.PropertyField(_contactDamageMaxProp);
        EditorGUILayout.PropertyField(_experienceDropProp);
        EditorGUILayout.PropertyField(_awarenessProp);
        EditorGUILayout.PropertyField(_agilityProp);
        EditorGUILayout.PropertyField(_enemyDeathEffect);

        _serializedScriptableObject.ApplyModifiedProperties();
    }

    private void SaveScriptableObject()
    {
        if (_newEnemyDetails != null)
        {
            string path = AssetDatabase.GenerateUniqueAssetPath(_assetPath + "EnemyDetails_" + _enemyType + ".asset");
            AssetDatabase.CreateAsset(_newEnemyDetails, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _newEnemyDetails;
        }
    }

    private void SaveEditedScriptableObject()
    {
        if (_selectedEnemyDetails != null)
        {
            EditorUtility.SetDirty(_selectedEnemyDetails);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _selectedEnemyDetails;
        }
    }

    private void CreatePrefabVariant()
    {
        _newPrefabVariantName = _enemyType;
        _tempPrefabVariant = (GameObject)PrefabUtility.InstantiatePrefab(_basePrefab); // temp
        _tempPrefabVariant.GetComponent<Enemy>().EnemyDetails = _newEnemyDetails;

        SavePrefabVariant();
    }

    private void SavePrefabVariant()
    {
        string path = "Assets/Prefabs/Enemies/Enemy_" + _newPrefabVariantName + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        _newPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(_tempPrefabVariant, path, InteractionMode.UserAction);
        
        DestroyImmediate(_tempPrefabVariant);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = _newPrefab;
    }

    private EnemySpawner AddEnemyToEnemySpawner(GameObject prefab)
    {
        EnemySpawner enemySpawner = _enemySpawnerPrefab.GetComponent<EnemySpawner>();
        enemySpawner.AddEnemyToEnemyList(prefab);

        return enemySpawner;
    }

    private void EditEnemySpawner(EnemySpawner enemySpawner)
    {
        if (_serializedEnemySpawnerObject == null || _serializedEnemySpawnerObject.targetObject != enemySpawner)
        {
            _serializedEnemySpawnerObject = new SerializedObject(enemySpawner);
            _enemyPrefabListProp = _serializedEnemySpawnerObject.FindProperty("_enemyPrefabList");
        }

        _serializedEnemySpawnerObject.Update();

        EditorGUILayout.PropertyField(_enemyPrefabListProp);

        _serializedEnemySpawnerObject.ApplyModifiedProperties();
    }

    private void SaveEnemySpawner()
    {
        if (_enemySpawnerPrefab != null)
        {
            EditorUtility.SetDirty(_enemySpawnerPrefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }

    private PoolManager AddEnemyToPoolManager(GameObject newPrefab)
    {
        PoolManager poolManager = _poolManagerPrefab.GetComponent<PoolManager>();
        poolManager.CreatePoolForEnemy(newPrefab);

        return poolManager;
    }

    private void EditPoolManager(PoolManager poolManager)
    {
        if (_serializedPoolManagerObject == null || _serializedPoolManagerObject.targetObject != poolManager)
        { 
            _serializedPoolManagerObject = new SerializedObject(poolManager);
            _poolListProp = _serializedPoolManagerObject.FindProperty("_poolList");
        }

        _serializedPoolManagerObject.Update();

        EditorGUILayout.PropertyField(_poolListProp);

        _serializedPoolManagerObject.ApplyModifiedProperties();
    }

    private void SavePoolManager()
    {
        if (_poolManagerPrefab != null)
        {
            EditorUtility.SetDirty(_poolManagerPrefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}
