using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "ScriptableObjects/Ammo/AmmoDetails")]
public class AmmoDetailsSO : ScriptableObject
{
    [Header("Basic Ammo Info & Cosmetics")]
    public string Name;
    public GameObject Prefab;

    [Header("Ammo Technical Specifications")]
    public float SpeedMin;
    public float SpeedMax;
    public int DamageMin;
    public int DamageMax;
    public float RangeMin;
    public float RangeMax;
    public float RotationSpeed;

    [Header("Ammo Spawn Details")]
    public int SpawnAmountMin;
    public int SpawnAmountMax;
    public float SpawnIntervalMin;
    public float SpawnIntervalMax;

    //[Header("Ammo Effects")]
    // write effectSOs here
}
