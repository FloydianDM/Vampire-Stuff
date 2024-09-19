using UnityEngine;

public class MovementByVelocity : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private MovementByVelocityEvent _movementByVelocityEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
    }

    private void OnEnable()
    {
        _movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void OnDisable()
    {
        _movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent @event, MovementByVelocityEventArgs args)
    {
        MovePlayer(args);
    }

    private void MovePlayer(MovementByVelocityEventArgs args)
    {
        _rigidbody.velocity = args.MoveDirection * args.MoveSpeed;
    }
}
