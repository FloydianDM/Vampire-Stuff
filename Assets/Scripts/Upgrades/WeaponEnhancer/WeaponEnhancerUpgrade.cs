using UnityEngine;

[RequireComponent(typeof(WeaponWorkshop))]
[DisallowMultipleComponent]
public class WeaponEnhancerUpgrade : Upgrade
{
    public WeaponEnhancerUpgradeDetailsSO WeaponEnhancerUpgradeDetails;
    [HideInInspector] public WeaponWorkshop WeaponWorkshop;

    protected override void Awake()
    {
        base.Awake();
        
        WeaponWorkshop = GetComponent<WeaponWorkshop>();
    }
}
