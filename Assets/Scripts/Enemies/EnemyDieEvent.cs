using System;
using UnityEngine;

public class EnemyDieEvent : MonoBehaviour
{
    public event Action<EnemyDieEvent, EnemyDieEventArgs> OnEnemyDie;

    public void CallEnemyDieEvent(float dieTime)
    {
        OnEnemyDie?.Invoke(this, new EnemyDieEventArgs()
        {
            DieTime = dieTime
        });
    }
}

public class EnemyDieEventArgs : EventArgs
{
    public float DieTime;
} 
