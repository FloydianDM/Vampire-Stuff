using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DestroyedEvent))]
[DisallowMultipleComponent]
public class Destroyed : MonoBehaviour
{
    private DestroyedEvent _destroyedEvent;
    private EnemyDetailsSO _enemyDetails;
    private Enemy _enemy;

    private GameManager _gameManager => GameManager.Instance;
    private PoolManager _poolManager => PoolManager.Instance;

    private void Awake()
    {
        _destroyedEvent = GetComponent<DestroyedEvent>();

        _enemy = GetComponent<Enemy>();

        if (_enemy != null)
        {
            _enemyDetails = _enemy.EnemyDetails;
        }

    }

    private void OnEnable() 
    {
        _destroyedEvent.OnDestroyedEvent += DestroyedEvent_OnDestroyedEvent;    
    }

    private void OnDisable()
    {
        _destroyedEvent.OnDestroyedEvent -= DestroyedEvent_OnDestroyedEvent;
    }

    private void DestroyedEvent_OnDestroyedEvent(DestroyedEvent @event, DestroyedEventArgs args)
    {
        if (args.IsPlayerDied)
        {
            _gameManager.RestartGame();

            return;
        }

        if (args.IsEnemyDied)
        {
            ExecuteEnemyDeath();

            return;
        }

        Destroy(gameObject);
    }

    private void ExecuteEnemyDeath()
    {
        EnemyDeathEffect enemyDeathEffect = 
            (EnemyDeathEffect)_poolManager.ReuseComponent(_enemyDetails.EnemyDeathEffect.Prefab, transform.position, Quaternion.identity);
        
        enemyDeathEffect.InitialiseEnemyDeathEffect(transform.position);

        enemyDeathEffect.PlayEnemyDeathEffect();
        gameObject.SetActive(false);
        
        _gameManager.Player.ReceiveXPEvent.CallReceiveXPEvent(_enemyDetails.ExperienceDrop);
    }
}
