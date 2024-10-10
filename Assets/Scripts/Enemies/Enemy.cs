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
    private HealthEvent _healthEvent;
    private SpriteRenderer _spriteRenderer;
    public Health Health { get; private set; }
    public EnemyMovement EnemyMovement { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public IdleEvent IdleEvent { get; private set; }
    public MovementByVelocityEvent MovementByVelocityEvent { get; private set; }
    public EnemyDieEvent EnemyDieEvent { get; private set; }
    public EnemyDodgeEvent EnemyDodgeEvent { get; private set; }
    public DestroyedEvent DestroyedEvent { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Health = GetComponent<Health>();
        _healthEvent = GetComponent<HealthEvent>();
        EnemyMovement = GetComponent<EnemyMovement>();
        Rigidbody = GetComponent<Rigidbody2D>();
        IdleEvent = GetComponent<IdleEvent>();
        MovementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        EnemyDieEvent = GetComponent<EnemyDieEvent>();
        EnemyDodgeEvent = GetComponent<EnemyDodgeEvent>();
        DestroyedEvent = GetComponent<DestroyedEvent>();

        _spriteRenderer.sprite = EnemyDetails.Sprite;
        _spriteRenderer.color = EnemyDetails.SpriteColor;
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
        switch (args.GameState)
        {
            case GameState.Pause:
            case GameState.LevelUp:
                Animator.enabled = false;
                break;
            case GameState.Play:
                Animator.enabled = true;
                break;
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

        Health.SetStartingHealth(enemyHealth);
    }

    private void DestroyEnemy()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(false, true);
    }
}
