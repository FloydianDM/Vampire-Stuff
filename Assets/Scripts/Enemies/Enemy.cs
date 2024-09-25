using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(AnimateEnemy))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(EnemyDodge))]
[RequireComponent(typeof(EnemyDodgeEvent))]
[RequireComponent(typeof(EnemyDie))]
[RequireComponent(typeof(EnemyDieEvent))]
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    public EnemyDetailsSO EnemyDetails;
    private Health _health;
    private HealthEvent _healthEvent;
    [HideInInspector] public EnemyMovement EnemyMovement;
    [HideInInspector] public Rigidbody2D Rigidbody;
    [HideInInspector] public IdleEvent IdleEvent;
    [HideInInspector] public MovementByVelocityEvent MovementByVelocityEvent;
    [HideInInspector] public EnemyDieEvent EnemyDieEvent;
    [HideInInspector] public EnemyDodgeEvent EnemyDodgeEvent;
    [HideInInspector] public Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _healthEvent = GetComponent<HealthEvent>();
        EnemyMovement = GetComponent<EnemyMovement>();
        Rigidbody = GetComponent<Rigidbody2D>();
        IdleEvent = GetComponent<IdleEvent>();
        MovementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        EnemyDieEvent = GetComponent<EnemyDieEvent>();
        EnemyDodgeEvent = GetComponent<EnemyDodgeEvent>();
    }
    
    private void OnEnable()
    {
        gameObject.tag = Settings.SPAWNED_ENEMY_TAG;

        _healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        gameObject.tag = Settings.ENEMY_TAG;

        _healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent @event, HealthEventArgs args)
    {
        if (args.HealthAmount <= 0)
        {
            DestroyEnemy();
        }
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        if (args.GameState == GameState.PauseMenu)
        {
            Animator.enabled = false;
        }
        else if (args.GameState == GameState.Play)
        {
            Animator.enabled = true;
        }
    }

    public void InitialiseEnemy(Vector2 spawnPosition)
    {
        gameObject.SetActive(true);

        transform.position = spawnPosition;

        SetEnemyStartingHealth();
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
}
