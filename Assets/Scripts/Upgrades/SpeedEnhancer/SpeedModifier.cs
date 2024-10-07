using UnityEngine;

[DisallowMultipleComponent]
public class SpeedModifier : MonoBehaviour
{
    private SpeedEnhancerUpgrade _speedEnhancerUpgrade;
    
    private GameManager _gameManager => GameManager.Instance;

    private void Awake()
    {
        _speedEnhancerUpgrade = GetComponent<SpeedEnhancerUpgrade>();
    }

    private void OnEnable()
    {
        _speedEnhancerUpgrade.SpeedEnhancingEvent.OnSpeedEnhancing += SpeedEnhancingEvent_OnSpeedEnhancing;
    }

    private void OnDisable()
    {
        _speedEnhancerUpgrade.SpeedEnhancingEvent.OnSpeedEnhancing -= SpeedEnhancingEvent_OnSpeedEnhancing;
    }

    private void SpeedEnhancingEvent_OnSpeedEnhancing(SpeedEnhancingEvent @event, SpeedEnhancingEventArgs args)
    {
        _gameManager.Player.Speed *= args.SpeedModifierValue;
    }
}
