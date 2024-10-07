using UnityEngine;

[RequireComponent(typeof(SpeedModifier))]
[DisallowMultipleComponent]
public class SpeedEnhancerUpgrade : Upgrade
{
    public SpeedEnhancerUpgradeDetailsSO SpeedEnhancerUpgradeDetails;
    [HideInInspector] public SpeedModifier SpeedModifier;

    protected override void Awake()
    {
        base.Awake();
        
        SpeedModifier = GetComponent<SpeedModifier>();
    }
}
