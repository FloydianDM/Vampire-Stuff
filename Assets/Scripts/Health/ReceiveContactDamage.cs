using UnityEngine;

[RequireComponent(typeof(CameraShake))]
[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class ReceiveContactDamage : MonoBehaviour
{
    private int _contactDamageAmount;
    private Health _health;
    private CameraShake _cameraShake;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _cameraShake = GetComponent<CameraShake>();
    }

    public void TakeContactDamage(int damageAmount = 0)
    {
        if (_contactDamageAmount > 0)
        {
            damageAmount = _contactDamageAmount;
        }
        
        _health.TakeDamage(damageAmount);
        _cameraShake.ShakeCamera();
    }
}
