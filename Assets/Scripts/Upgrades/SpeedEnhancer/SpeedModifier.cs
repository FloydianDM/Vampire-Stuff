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

    public void EnhancePlayerSpeed()
    {
        _gameManager.Player.IncreasePlayerSpeed(_speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails.SpeedModifier);
    }
}
