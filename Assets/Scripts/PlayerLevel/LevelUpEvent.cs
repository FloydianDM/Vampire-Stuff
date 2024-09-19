using System;
using UnityEngine;

public class LevelUpEvent : MonoBehaviour
{
    public event Action<LevelUpEvent, LevelUpEventArgs> OnLevelUpEvent;

    public void CallLevelUpEvent(int playerLevel)
    {
        OnLevelUpEvent?.Invoke(this, new LevelUpEventArgs()
        {
            PlayerLevel = playerLevel
        });
    }
}

public class LevelUpEventArgs : EventArgs
{
    public int PlayerLevel;
}
