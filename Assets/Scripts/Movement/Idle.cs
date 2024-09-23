using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
[DisallowMultipleComponent]
public class Idle : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private IdleEvent _idleEvent;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _idleEvent = GetComponent<IdleEvent>();
    }

    private void OnEnable()
    {
        _idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable()
    {
        _idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void IdleEvent_OnIdle(IdleEvent @event)
    {
        _rigidBody.velocity = Vector2.zero;
    }
}
