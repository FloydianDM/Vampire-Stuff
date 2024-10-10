using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ChestSpawnEvent))]
[DisallowMultipleComponent]
public class ChestSpawner : SingletonMonobehaviour<ChestSpawner>
{
    [SerializeField] private Transform[] _chestSpawnLocationArray;
    [SerializeField] private ChestDetailsSO[] _chestDetailsArray;

    private int _defaultSpawnNumber = 1;
    private int _maximumSpawnNumber = 10;

    private bool _shouldSpawn = true;

    [HideInInspector] public ChestSpawnEvent ChestSpawnEvent;
    private PoolManager _poolManager => PoolManager.Instance;

    protected override void Awake()
    {
        base.Awake();

        ChestSpawnEvent = GetComponent<ChestSpawnEvent>();
    }

    private void Start()
    {
        SpawnChests(_defaultSpawnNumber);
    }

    private void OnEnable()
    {
        ChestSpawnEvent.OnChestSpawn += ChestSpawnEvent_OnChestSpawn;
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        ChestSpawnEvent.OnChestSpawn -= ChestSpawnEvent_OnChestSpawn;
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void ChestSpawnEvent_OnChestSpawn(ChestSpawnEvent @event, ChestSpawnEventArgs args)
    {
        SpawnChests(args.ChestSpawnNumber);
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
                break;
        }
    }

    private void SpawnChests(int spawnNumber)
    {
        StartCoroutine(SpawnChestsRoutine(spawnNumber));
    }

    private IEnumerator SpawnChestsRoutine(int spawnNumber)
    {
        int spawnCount = 0;

        if (spawnNumber > _maximumSpawnNumber)
        {
            spawnNumber = _maximumSpawnNumber;
        }
        
        if (_chestSpawnLocationArray.Length > 0)
        {
            while (spawnCount < spawnNumber)
            {
                while (!_shouldSpawn)
                { 
                    yield return null;
                }

                Vector2 spawnLocation = _chestSpawnLocationArray[Random.Range(0, _chestSpawnLocationArray.Length)].position;
                ChestDetailsSO chestDetails = _chestDetailsArray[Random.Range(0, _chestDetailsArray.Length)];

                // create chest
                CreateChest(chestDetails, spawnLocation);

                spawnCount++;
                
                yield return new WaitForSeconds(20f);
            }
        }
    }

    private void CreateChest(ChestDetailsSO chestDetails, Vector2 spawnLocation)
    {
        GameObject chestPrefab = chestDetails.Prefab;
        IUsable chest = (IUsable)_poolManager.ReuseComponent(chestPrefab, spawnLocation, Quaternion.identity);
        chest.InitializeUsable(chestDetails);
    }
}
