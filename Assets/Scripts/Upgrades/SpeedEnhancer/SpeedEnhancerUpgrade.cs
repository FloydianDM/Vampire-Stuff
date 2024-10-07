using UnityEngine;

[RequireComponent(typeof(SpeedEnhancingEvent))]
[RequireComponent(typeof(SpeedModifier))]
[DisallowMultipleComponent]
public class SpeedEnhancerUpgrade : Upgrade
{
    public SpeedEnhancerUpgradeDetailsSO SpeedEnhancerUpgradeDetails;
    [HideInInspector] public SpeedEnhancingEvent SpeedEnhancingEvent;
    [HideInInspector] public SpeedModifier SpeedModifier;
    [HideInInspector] public float SpeedModifierValue;

    protected override void Awake()
    {
        base.Awake();

        SpeedEnhancingEvent = GetComponent<SpeedEnhancingEvent>();
        SpeedModifier = GetComponent<SpeedModifier>();
        SpeedModifierValue = SpeedEnhancerUpgradeDetails.SpeedModifier;
    }
}
