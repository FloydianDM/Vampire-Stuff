using System;
using UnityEngine;

public class BombActivationEvent : MonoBehaviour
{
    public event Action<BombActivationEvent> OnBombActivated;

    public void CallBombActivationEvent()
    {
        OnBombActivated?.Invoke(this);
    }
    
}
