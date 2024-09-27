using UnityEngine;

[CreateAssetMenu(fileName = "ChestDetailsSO_", menuName = "ScriptableObjects/Chests/ChestDetails")]
public class ChestDetailsSO : ScriptableObject
{
    public GameObject Prefab;
    public int HealthPercent;
    public WeaponDetailsSO WeaponDetails;
    public int AddedXP;
}

