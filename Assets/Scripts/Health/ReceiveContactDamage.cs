using UnityEngine;

[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class ReceiveContactDamage : MonoBehaviour
{
    [SerializeField] private int _contactDamageAmount;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void TakeContactDamage(int damageAmount = 0)
    {
        if (_contactDamageAmount > 0)
        {
            damageAmount = _contactDamageAmount;
        }
        
        _health.TakeDamage(damageAmount);
    }
}
