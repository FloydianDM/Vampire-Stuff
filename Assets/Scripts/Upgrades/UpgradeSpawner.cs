using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeSpawner : SingletonMonobehaviour<UpgradeSpawner>
{
    [SerializeField] private List<GameObject> _bombList;
    [SerializeField] private List<GameObject> _weaponEnhancerList;
    [SerializeField] private List<GameObject> _speedEnhancerList;
    
    public GameObject FillBombSlot()
    {
        int randomIndex = Random.Range(0, _bombList.Count);

        return _bombList[randomIndex];
    }

    public GameObject FillWeaponEnhancerSlot()
    {
        int randomIndex = Random.Range(0, _weaponEnhancerList.Count);

        return _weaponEnhancerList[randomIndex];
    }
    
    public GameObject FillSpeedEnhancerSlot()
    {
        int randomIndex = Random.Range(0, _speedEnhancerList.Count);
        
        return _speedEnhancerList[randomIndex];
    }
    
    public void SpawnWeaponEnhancer(GameObject weaponEnhancerGameObject)
    {
        GameObject weaponEnhancerObject = Instantiate(weaponEnhancerGameObject, transform);
        WeaponEnhancerUpgrade weaponEnhancerUpgrade = weaponEnhancerObject.GetComponent<WeaponEnhancerUpgrade>();
        
        weaponEnhancerUpgrade.WeaponWorkshop.EnhanceWeapon();
        
        Destroy(weaponEnhancerObject);
    }

    public void SpawnSpeedEnhancer(GameObject speedEnhancerGameObject)
    {
        GameObject speedEnhancerObject = Instantiate(speedEnhancerGameObject, transform);
        SpeedEnhancerUpgrade speedEnhancerUpgrade = speedEnhancerObject.GetComponent<SpeedEnhancerUpgrade>();

        speedEnhancerUpgrade.SpeedModifier.EnhancePlayerSpeed();
        
        Destroy(speedEnhancerObject);
    }
}
