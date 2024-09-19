using UnityEngine;

[DisallowMultipleComponent]
public class DealContactDamage : MonoBehaviour
{
    [SerializeField] private int _contactDamageAmount;

    private bool _isColliding = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isColliding)
        {
            return;
        }

        ContactDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isColliding)
        {
            return;
        }

        ContactDamage(other);
    }

    private void ContactDamage(Collider2D other)
    {
        ReceiveContactDamage receiveContactDamage = other.gameObject.GetComponent<ReceiveContactDamage>();

        if (receiveContactDamage != null)
        {
            _isColliding = true;

            receiveContactDamage.TakeContactDamage(_contactDamageAmount);

            Invoke(nameof(ResetContactCollision), Settings.RESET_CONTACT_COLLISION_TIME);
        }
    }

    private void ResetContactCollision()
    {
        _isColliding = false;
    }
    
}
