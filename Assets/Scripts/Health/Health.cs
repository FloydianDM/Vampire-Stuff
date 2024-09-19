using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public int StartingHealth { get; private set; }
    public int MaxHealth { get; private set; }
    private int _currentHealth;
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
        _currentHealth = MaxHealth;

        CallHealthEvent(0);
    }

    public void TakeDamage(int damageAmount)
    {
        if (_player != null || Enemy != null)
        {
            _currentHealth -= damageAmount;
            
            CallHealthEvent(damageAmount);
        }
    }

    private void CallHealthEvent(int damageAmount)
    {
        _healthEvent.CallHealthChangedEvent((float)_currentHealth / MaxHealth, _currentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {  
        StartingHealth = startingHealth;
        _currentHealth = StartingHealth;  
        MaxHealth = startingHealth;
    }
}
