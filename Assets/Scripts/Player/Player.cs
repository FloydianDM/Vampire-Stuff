using System;
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
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform WeaponTransform;
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
    [HideInInspector] public List<Weapon> WeaponList = new List<Weapon>();
    [HideInInspector] public Health Health;

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
        if (args.GameState == GameState.PauseMenu)
        {
            Animator.enabled = false;
        }
        else if (args.GameState == GameState.Play)
        {
            Animator.enabled = true;
        }
    }

    public void InitialisePlayer(PlayerDetailsSO playerDetails)
    {
        PlayerDetails = playerDetails;
        SetPlayerHealth();
        SetStartingWeapon();
    }

    private void SetPlayerHealth()
    {
        Health.SetStartingHealth(PlayerDetails.Health);
    }

    private void SetStartingWeapon()
    {
        WeaponList.Clear();
        WeaponList.Add(PlayerDetails.StartingWeaponDetails.Weapon);
        Instantiate(PlayerDetails.StartingWeaponDetails.Weapon, WeaponTransform);
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
}
