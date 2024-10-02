using UnityEngine;

[CreateAssetMenu(fileName = "BombUpgradeDetails_", menuName = "ScriptableObjects/Upgrades/Bombs/BombUpgradeDetails")]
public class BombUpgradeDetailsSO : UpgradeDetailsSO
{
    public float ImpactArea;
    public float CooldownTime;
    public float Power;
}
