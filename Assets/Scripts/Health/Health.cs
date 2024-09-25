using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public int StartingHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    private HealthEvent _healthEvent;
    private Player _player;
    private float _levelUpMultiplier = 0.6f;

    [HideInInspector] public bool IsDamageable = true;
    [HideInInspector] public Enemy Enemy;

    private void Awake()
    {
        _healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        _player = GetComponent<Player>();
        Enemy = GetComponent<Enemy>();

        if (_player != null)
        {
            _player.LevelUpEvent.OnLevelUpEvent += LevelUpEvent_OnLevelUpEvent;
        }
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.LevelUpEvent.OnLevelUpEvent -= LevelUpEvent_OnLevelUpEvent;
        }
    }

    private void LevelUpEvent_OnLevelUpEvent(LevelUpEvent @event, LevelUpEventArgs args)
    {
        MaxHealth = Mathf.RoundToInt(StartingHealth * args.PlayerLevel * _levelUpMultiplier);
        CurrentHealth = MaxHealth;

        CallHealthEvent(0);
    }

    public void TakeDamage(int damageAmount)
    {
        if (_player != null || Enemy != null)
        {
            CurrentHealth -= damageAmount;
            
            CallHealthEvent(damageAmount);
        }
    }

    private void CallHealthEvent(int damageAmount)
    {
        _healthEvent.CallHealthChangedEvent((float)CurrentHealth / MaxHealth, CurrentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {  
        StartingHealth = startingHealth;
        CurrentHealth = StartingHealth;  
        MaxHealth = startingHealth;
    }

    public void AddHealth(int healthPercent)
    {
        int healthIncrease = Mathf.RoundToInt(MaxHealth * healthPercent / 100);
        int totalHealth = CurrentHealth + healthIncrease;

        totalHealth = Mathf.Clamp(totalHealth, 0, MaxHealth);

        CurrentHealth = totalHealth;

        CallHealthEvent(0);
    }
}
