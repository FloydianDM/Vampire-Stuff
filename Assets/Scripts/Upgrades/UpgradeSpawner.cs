using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeSpawner : SingletonMonobehaviour<UpgradeSpawner>
{
    [SerializeField] private List<GameObject> _bombList;

    public GameObject FillBombSlot()
    {
        int randomIndex = Random.Range(0, _bombList.Count);

        return _bombList[randomIndex];
    }
}
