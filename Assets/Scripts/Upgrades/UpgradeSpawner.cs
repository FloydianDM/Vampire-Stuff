using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeSpawner : SingletonMonobehaviour<UpgradeSpawner>
{
    [SerializeField] private List<GameObject> _bombList;
    [SerializeField] private List<GameObject> _speedEnhancerList;
    
    public GameObject FillBombSlot()
    {
        int randomIndex = Random.Range(0, _bombList.Count);

        return _bombList[randomIndex];
    }

    public GameObject FillSpeedEnhancerSlot()
    {
        int randomIndex = Random.Range(0, _speedEnhancerList.Count);
        
        return _speedEnhancerList[randomIndex];
    }

    public void SpawnSpeedEnhancer(GameObject speedEnhancerGameObject)
    {
        GameObject speedEnhancerObject = Instantiate(speedEnhancerGameObject, transform);
        SpeedEnhancerUpgrade speedEnhancerUpgrade = speedEnhancerObject.GetComponent<SpeedEnhancerUpgrade>();
        
        speedEnhancerUpgrade.SpeedEnhancingEvent.CallSpeedEnhancingEvent(speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails.SpeedModifier);
        
        Destroy(speedEnhancerObject);
    }
}
