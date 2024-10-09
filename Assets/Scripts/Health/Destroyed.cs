using UnityEngine;

[RequireComponent(typeof(DestroyedEvent))]
[DisallowMultipleComponent]
public class Destroyed : MonoBehaviour
{
    private DestroyedEvent _destroyedEvent;
    private Enemy _enemy;
    
    private GameManager _gameManager => GameManager.Instance;
    private ScoreManager _scoreManager => ScoreManager.Instance;

    private void Awake()
    {
        _destroyedEvent = GetComponent<DestroyedEvent>();
        _enemy = GetComponent<Enemy>();
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
            ExecutePlayerDeath();

            return;
        }

        if (args.IsEnemyDied)
        {
            ExecuteEnemyDeath();

            return;
        }

        Destroy(gameObject);
    }

    private void ExecutePlayerDeath()
    {
        _scoreManager.SaveHighScore();
        _gameManager.RestartGame();
    }

    private void ExecuteEnemyDeath()
    {
        _enemy.EnemyDieEvent.CallEnemyDieEvent(1f);
    }
}
