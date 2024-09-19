using UnityEngine;

[RequireComponent(typeof(FireWeapon))]
[DisallowMultipleComponent]
public class Weapon : MonoBehaviour
{
    public WeaponDetailsSO WeaponDetails;
    public int WeaponListPosition;
    public Transform ShootPosition;
}
