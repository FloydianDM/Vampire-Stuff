using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(EnemyMovement))]
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    public EnemyDetailsSO EnemyDetails;
    private Health _health;
    private HealthEvent _healthEvent;
    [HideInInspector] public EnemyMovement EnemyMovement;
    [HideInInspector] public Rigidbody2D Rigidbody;

    private GameResources _gameResources => GameResources.Instance;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _healthEvent = GetComponent<HealthEvent>();
        EnemyMovement = GetComponent<EnemyMovement>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        gameObject.tag = Settings.SPAWNED_ENEMY_TAG;
        _healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        gameObject.tag = Settings.ENEMY_TAG;
        _healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    public void InitialiseEnemy(Vector2 spawnPosition)
    {
        gameObject.SetActive(true);

        transform.position = spawnPosition;

        SetEnemyStartingHealth();
    }

    private void HealthEvent_OnHealthChanged(HealthEvent @event, HealthEventArgs args)
    {
        if (args.HealthAmount <= 0)
        {
            DestroyEnemy();
        }
    }    

    private void SetEnemyStartingHealth()
    {
        int enemyHealth = Random.Range(EnemyDetails.HealthMin, EnemyDetails.HealthMax + 1);

        _health.SetStartingHealth(enemyHealth);
    }

    private void DestroyEnemy()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(false, true);
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }
}
