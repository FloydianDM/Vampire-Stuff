using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyDieEvent))]
[DisallowMultipleComponent]
public class EnemyDie : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyDieEvent _enemyDieEvent;

    private PoolManager _poolManager => PoolManager.Instance;
    private EnemySpawner _enemySpawner => EnemySpawner.Instance;
    private ScoreManager _scoreManager => ScoreManager.Instance;
    private GameManager _gameManager => GameManager.Instance;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemyDieEvent = GetComponent<EnemyDieEvent>();
    }

    private void OnEnable()
    {
        _enemyDieEvent.OnEnemyDie += EnemyDieEvent_OnEnemyDie;
    }

    private void OnDisable()
    {
        _enemyDieEvent.OnEnemyDie -= EnemyDieEvent_OnEnemyDie;
    }

    private void EnemyDieEvent_OnEnemyDie(EnemyDieEvent @event, EnemyDieEventArgs args)
    {
        StartCoroutine(EnemyDieRoutine(args.DieTime));
    }

    private IEnumerator EnemyDieRoutine(float dieTime)
    {
        yield return new WaitForSeconds(dieTime);
        
        _gameManager.Player.ReceiveXPEvent.CallReceiveXPEvent(_enemy.EnemyDetails.ExperienceDrop);
        _scoreManager.ScoreChangedEvent.CallScoreChangedEvent(_enemy.Health.StartingHealth);
        
        EnemyDeathEffect enemyDeathEffect = 
            (EnemyDeathEffect)_poolManager.ReuseComponent(
                _enemy.EnemyDetails.EnemyDeathEffect.Prefab, transform.position, Quaternion.identity);
        
        enemyDeathEffect.InitialiseEnemyDeathEffect(transform.position);
        enemyDeathEffect.PlayEnemyDeathEffect();
        _enemySpawner.ReduceEnemyCount();

        gameObject.SetActive(false);
    }
}
