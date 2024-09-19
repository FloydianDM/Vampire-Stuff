using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ReceiveXPEvent : MonoBehaviour
{
    public event Action<ReceiveXPEvent, ReceiveXPEventArgs> OnReceiveXPEvent;

    public void CallReceiveXPEvent(int xpAmount)
    {
        OnReceiveXPEvent?.Invoke(this, new ReceiveXPEventArgs()
        {
            XPAmount = xpAmount
        });
    }
}

public class ReceiveXPEventArgs : EventArgs
{
    public int XPAmount;
}
