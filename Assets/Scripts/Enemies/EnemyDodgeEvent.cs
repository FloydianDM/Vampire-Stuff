using System;
using UnityEngine;

public class EnemyDodgeEvent : MonoBehaviour
{
    public event Action<EnemyDodgeEvent, EnemyDodgeEventArgs> OnEnemyDodge;

    public void CallEnemyDodgeEvent(float dodgeTime, float dodgeThrust)
    {
        OnEnemyDodge?.Invoke(this, new EnemyDodgeEventArgs()
        {
            DodgeTime = dodgeTime,
            DodgeThrust = dodgeThrust
        });
    }
}

public class EnemyDodgeEventArgs : EventArgs
{
    public float DodgeTime;
    public float DodgeThrust;
}
