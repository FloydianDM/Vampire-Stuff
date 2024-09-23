using UnityEngine;

[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _enemy.MovementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        _enemy.IdleEvent.OnIdle += OnIdleEvent_OnIdle;
        _enemy.EnemyDieEvent.OnEnemyDie += OnEnemyDieEvent_OnEnemyDie;
    }

    private void OnDisable()
    {
        _enemy.MovementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        _enemy.IdleEvent.OnIdle -= OnIdleEvent_OnIdle;
        _enemy.EnemyDieEvent.OnEnemyDie -= OnEnemyDieEvent_OnEnemyDie;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent @event, MovementByVelocityEventArgs args)
    {
        _enemy.Animator.SetBool(Settings.IsIdle, false);
        _enemy.Animator.SetBool(Settings.IsMoving, true);
    }

    private void OnIdleEvent_OnIdle(IdleEvent @event)
    {
        _enemy.Animator.SetBool(Settings.IsMoving, false);
        _enemy.Animator.SetBool(Settings.IsIdle, true);
    }
    
    private void OnEnemyDieEvent_OnEnemyDie(EnemyDieEvent @event, EnemyDieEventArgs args)
    {   
        _enemy.Animator.SetBool(Settings.IsMoving, false);
        _enemy.Animator.SetBool(Settings.IsIdle, false);
        _enemy.Animator.SetBool(Settings.IsDying, true);
    }

}
