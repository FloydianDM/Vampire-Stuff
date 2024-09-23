using UnityEngine;

[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(EnemyFinder))]
[RequireComponent(typeof(ReceiveXP))]
[RequireComponent(typeof(ReceiveXPEvent))]
[RequireComponent(typeof(PlayerLevelManager))]
[RequireComponent(typeof(LevelUpEvent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimatePlayer))]
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerControls PlayerControls;
    [HideInInspector] public MovementByVelocityEvent MovementToVelocityEvent;
    [HideInInspector] public PlayerDetailsSO PlayerDetails;
    [HideInInspector] public HealthEvent HealthEvent;
    [HideInInspector] public IdleEvent IdleEvent;
    [HideInInspector] public ReceiveXP ReceiveXP;
    [HideInInspector] public ReceiveXPEvent ReceiveXPEvent;
    [HideInInspector] public PlayerLevelManager PlayerLevelManager;
    [HideInInspector] public LevelUpEvent LevelUpEvent;
    [HideInInspector] public Animator Animator;

    private Health _health;

    private void Awake()
    {
        PlayerControls = GetComponent<PlayerControls>();
        MovementToVelocityEvent = GetComponent<MovementByVelocityEvent>();
        _health = GetComponent<Health>();
        HealthEvent = GetComponent<HealthEvent>();
        IdleEvent = GetComponent<IdleEvent>();
        ReceiveXP = GetComponent<ReceiveXP>();
        ReceiveXPEvent = GetComponent<ReceiveXPEvent>();
        PlayerLevelManager = GetComponent<PlayerLevelManager>();
        LevelUpEvent = GetComponent<LevelUpEvent>();
        Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent @event, HealthEventArgs args)
    {
        if (args.HealthAmount < 0)
        {
            DestroyPlayer();
        }
    }

    public void InitialisePlayer(PlayerDetailsSO playerDetails)
    {
        PlayerDetails = playerDetails;
        SetPlayerHealth();
    }

    private void SetPlayerHealth()
    {
        _health.SetStartingHealth(PlayerDetails.Health);
    }

    private void DestroyPlayer()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(true, false);
    }
}
