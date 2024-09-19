using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyFinder : MonoBehaviour
{
    private List<Enemy> _enemyList = new List<Enemy>();
    private float _listFillerTimer = 10f;
    private float _searchDiameter = 20f; // change it later

    private void Update()
    {
        FillEnemyList();
    }

    private void FillEnemyList()
    {
        StartCoroutine(FillEnemyListRoutine());     
    }

    private IEnumerator FillEnemyListRoutine()
    {
        _enemyList.Clear();

        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag(Settings.SPAWNED_ENEMY_TAG);

        foreach (GameObject gameObject in enemyArray)
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();

            _enemyList.Add(enemy);
        }

        yield return new WaitForSeconds(_listFillerTimer);
    }

    public Enemy FindClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;

        foreach (Enemy enemy in _enemyList)
        {
            float enemyDistance = Vector2.Distance(transform.position, enemy.gameObject.transform.position);

            if (enemyDistance < _searchDiameter)
            {
                if (closestEnemy == null || enemyDistance < closestEnemyDistance)
                {
                    closestEnemy = enemy;
                    closestEnemyDistance = enemyDistance;
                }
            }
        }
        
        return closestEnemy;
    }
}
