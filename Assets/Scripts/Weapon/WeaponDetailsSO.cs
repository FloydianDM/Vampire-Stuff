using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "ScriptableObjects/Weapon/WeaponDetails")]
public class WeaponDetailsSO : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public AmmoDetailsSO WeaponAmmo;
    public float FireRate;
    public bool IsAimable;
}
