using System;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthEvent : MonoBehaviour
{
    public event Action<HealthEvent, HealthEventArgs> OnHealthChanged;

    public void CallHealthChangedEvent(float healthPercent, int healthAmount, int damageAmount)
    {
        OnHealthChanged?.Invoke(this, new HealthEventArgs()
        {
            HealthAmount = healthAmount,
            HealthPercent = healthPercent,
            DamageAmount = damageAmount
        });
    }
}

public class HealthEventArgs : EventArgs
{
    public float HealthPercent;
    public int HealthAmount;
    public int DamageAmount;
}
