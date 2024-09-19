using System;
using UnityEngine;

public class MovementByVelocityEvent : MonoBehaviour
{
    public event Action<MovementByVelocityEvent, MovementByVelocityEventArgs> OnMovementByVelocity;

    public void CallMovementToVelocityEvent(float moveSpeed, Vector2 moveDirection)
    {
        OnMovementByVelocity?.Invoke(this, new MovementByVelocityEventArgs
            {
                MoveSpeed = moveSpeed,
                MoveDirection = moveDirection 
            });
    }
}

public class MovementByVelocityEventArgs : EventArgs
{
    public float MoveSpeed;
    public Vector2 MoveDirection;
}

