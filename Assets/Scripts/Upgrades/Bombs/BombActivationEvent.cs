using System;
using UnityEngine;

public class BombActivationEvent : MonoBehaviour
{
    public event Action<BombActivationEvent, BombActivationEventArgs> OnBombActivated;

    public void CallBombActivationEvent()
    {
        OnBombActivated?.Invoke(this, new BombActivationEventArgs()
        {
            
        });
    }
    
}

public class BombActivationEventArgs : EventArgs
{
    
}
