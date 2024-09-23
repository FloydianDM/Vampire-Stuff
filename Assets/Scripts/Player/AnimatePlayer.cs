using UnityEngine;

[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _player.MovementToVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        _player.IdleEvent.OnIdle += OnIdleEvent_OnIdle;
    }

    private void OnDisable()
    {
        _player.MovementToVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        _player.IdleEvent.OnIdle -= OnIdleEvent_OnIdle;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent @event, MovementByVelocityEventArgs args)
    {
        SetMovementAnimationParameters();
    }

    private void OnIdleEvent_OnIdle(IdleEvent @event)
    {
        SetIdleAnimationParameters();
    }

    private void SetMovementAnimationParameters()
    {
        _player.Animator.SetBool(Settings.IsMoving, true);
        _player.Animator.SetBool(Settings.IsIdle, false);
    }

    private void SetIdleAnimationParameters()
    {
        _player.Animator.SetBool(Settings.IsIdle, true);
        _player.Animator.SetBool(Settings.IsMoving, false);
    }
}
