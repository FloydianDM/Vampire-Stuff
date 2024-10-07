using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    [SerializeField] private Transform[] _spawnPositionArray;
    public List<GameObject> EnemyPrefabList;
    [SerializeField] private int _enemiesToSpawn = 1000;

    private int _maxEnemySpawn = 1000;
    private int _spawnedEnemyCount;
    private float _timeBetweenSpawns = 1.8f;
    private float _spawnTimeModifier = 0.1f;
    private float _enemySpawnerDelay = 1f;
    private bool _shouldSpawn = true;

    private PoolManager _poolManager => PoolManager.Instance;
    private GameManager _gameManager => GameManager.Instance;

    private void Start()
    {
        //SpawnEnemies();
        _gameManager.Player.LevelUpEvent.OnLevelUpEvent += LevelUpEvent_OnLevelUpEvent;
    }

    private void OnEnable()
    {
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
        _gameManager.Player.LevelUpEvent.OnLevelUpEvent -= LevelUpEvent_OnLevelUpEvent;
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        switch (args.GameState)
        {
            case GameState.Pause:
            case GameState.LevelUp:
                _shouldSpawn = false;
                break;
            case GameState.Play:
                _shouldSpawn = true;
                SpawnEnemies();
                break;
        }
    }
    private void LevelUpEvent_OnLevelUpEvent(LevelUpEvent @event, LevelUpEventArgs args)
    {
        if (_timeBetweenSpawns <= 0.1f)
        {
            return;
        }

        _timeBetweenSpawns -= _spawnTimeModifier;
    }

    private void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(_enemySpawnerDelay);
        
        if (_spawnPositionArray.Length > 0)
        {
            while (_shouldSpawn)
            {
                if (_spawnedEnemyCount < _enemiesToSpawn)
                {
                    Vector2 spawnPosition = _spawnPositionArray[Random.Range(0, _spawnPositionArray.Length)].position;
                    GameObject enemyPrefab = EnemyPrefabList[Random.Range(0, EnemyPrefabList.Count)];

                    CreateEnemy(enemyPrefab, spawnPosition);

                    yield return new WaitForSeconds(_timeBetweenSpawns);
                }
                else
                {
                    yield return null;
                }
            }    
        }
    }

    private void CreateEnemy(GameObject enemyPrefab, Vector2 spawnPosition)
    {
        _spawnedEnemyCount++;

        Enemy enemy = (Enemy)_poolManager.ReuseComponent(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.InitialiseEnemy(spawnPosition);
    }

    public void ReduceEnemyCount()
    {
        _spawnedEnemyCount--;
    }

    public void IncreaseEnemySpawnNumber(int increasedNumber)
    {
        if (_enemiesToSpawn <= _maxEnemySpawn - increasedNumber)
        {
            _enemiesToSpawn += increasedNumber;
        }
    }
}
