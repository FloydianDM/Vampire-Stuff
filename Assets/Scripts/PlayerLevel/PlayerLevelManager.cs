using UnityEngine;

[RequireComponent(typeof(LevelUpEvent))]
[DisallowMultipleComponent]
public class PlayerLevelManager : MonoBehaviour
{
    public int PlayerLevel { get; private set; }
    private int _startingPlayerLevel = 1;
    private LevelUpEvent _levelUpEvent;
    private int _increasedEnemyNumberForEachLevel = 10;

    private EnemySpawner _enemySpawner => EnemySpawner.Instance;
    private ChestSpawner _chestSpawner => ChestSpawner.Instance;

    private void Awake()
    {
        _levelUpEvent = GetComponent<LevelUpEvent>();

        PlayerLevel = _startingPlayerLevel;
    }

    public void GainLevelUp()
    {
        PlayerLevel++;

        _levelUpEvent.CallLevelUpEvent(PlayerLevel);
        StaticEventHandler.CallCombatNotifiedEvent("LEVELED UP!", 1f);
        _enemySpawner.IncreaseEnemySpawnNumber(_increasedEnemyNumberForEachLevel);
        _chestSpawner.ChestSpawnEvent.CallChestSpawnEvent(PlayerLevel);
    }
}
