using System;
using UnityEngine;

public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyedEvent;

    public void CallDestroyedEvent(bool isPlayerDied, bool isEnemyDied)
    {
        OnDestroyedEvent?.Invoke(this, new DestroyedEventArgs()
        {
            IsPlayerDied = isPlayerDied,
            IsEnemyDied = isEnemyDied
        });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool IsPlayerDied;
    public bool IsEnemyDied;
}
