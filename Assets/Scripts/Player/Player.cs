using System.Collections.Generic;
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
[RequireComponent(typeof(BombOperator))]
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _weaponTransform;

    private PlayerDetailsSO _playerDetails;
    public PlayerControls PlayerControls { get; private set; }
    public MovementByVelocityEvent MovementToVelocityEvent { get; private set; }
    public HealthEvent HealthEvent { get; private set; }
    public IdleEvent IdleEvent { get; private set; }
    public ReceiveXP ReceiveXP { get; private set; }
    public ReceiveXPEvent ReceiveXPEvent { get; private set; }
    public PlayerLevelManager PlayerLevelManager { get; private set; }
    public LevelUpEvent LevelUpEvent { get; private set; }
    public Animator Animator { get; private set; }
    public List<Weapon> WeaponList { get; } = new List<Weapon>();
    public Health Health { get; private set; }
    public BombOperator BombOperator { get; private set; }
    public float Speed { get; private set; }

    private void Awake()
    {
        PlayerControls = GetComponent<PlayerControls>();
        MovementToVelocityEvent = GetComponent<MovementByVelocityEvent>();
        Health = GetComponent<Health>();
        HealthEvent = GetComponent<HealthEvent>();
        IdleEvent = GetComponent<IdleEvent>();
        ReceiveXP = GetComponent<ReceiveXP>();
        ReceiveXPEvent = GetComponent<ReceiveXPEvent>();
        PlayerLevelManager = GetComponent<PlayerLevelManager>();
        LevelUpEvent = GetComponent<LevelUpEvent>();
        Animator = GetComponent<Animator>();
        BombOperator = GetComponent<BombOperator>();
    }

    private void OnEnable()
    {
        HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent @event, HealthEventArgs args)
    {
        if (args.HealthAmount < 0)
        {
            DestroyPlayer();
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

    public void InitialisePlayer(PlayerDetailsSO playerDetails)
    {
        _playerDetails = playerDetails;
        SetPlayerHealth();
        SetStartingWeapon();
        Speed = _playerDetails.Speed;
    }

    private void SetPlayerHealth()
    {
        Health.SetStartingHealth(_playerDetails.Health);
    }

    private void SetStartingWeapon()
    {
        WeaponList.Clear();
        WeaponList.Add(_playerDetails.StartingWeaponDetails.Weapon);
        Instantiate(_playerDetails.StartingWeaponDetails.Weapon, _weaponTransform);
    }

    private void DestroyPlayer()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(true, false);
    }

    public void AddWeaponToPlayerWeaponList(WeaponDetailsSO weaponDetails)
    {
        Weapon weapon = weaponDetails.Weapon;

        WeaponList.Add(weapon);
        weapon.WeaponListPosition = WeaponList.Count;
        Instantiate(weapon, _weaponTransform);
    }

    public bool IsWeaponHeldByPlayer(WeaponDetailsSO weaponDetails)
    {
        foreach (Weapon weapon in WeaponList)
        {
            if (weaponDetails == weapon.WeaponDetails)
            {
                return true;
            }
        }

        return false;
    }

    public Vector2 GetPlayerPosition()
    {
        return transform.position;
    }

    public void IncreasePlayerSpeed(float increaseModifier)
    {
        Speed *= increaseModifier;
    }
}
