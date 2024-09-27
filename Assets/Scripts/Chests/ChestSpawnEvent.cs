using System;
using UnityEngine;

public class ChestSpawnEvent : MonoBehaviour
{
    public event Action<ChestSpawnEvent, ChestSpawnEventArgs> OnChestSpawn;

    public void CallChestSpawnEvent(int chestSpawnNumber)
    {
        OnChestSpawn?.Invoke(this, new ChestSpawnEventArgs()
            {
                ChestSpawnNumber = chestSpawnNumber
            }
        );
    }
}

public class ChestSpawnEventArgs : EventArgs
{
    public int ChestSpawnNumber;
}
