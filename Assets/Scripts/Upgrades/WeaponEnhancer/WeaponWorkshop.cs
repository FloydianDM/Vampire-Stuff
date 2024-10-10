using UnityEngine;

[DisallowMultipleComponent]
public class WeaponWorkshop : MonoBehaviour
{
   private WeaponEnhancerUpgrade _weaponEnhancerUpgrade;
   private GameManager _gameManager => GameManager.Instance;

   private void Awake()
   {
      _weaponEnhancerUpgrade = GetComponent<WeaponEnhancerUpgrade>();
   }
   
   public void EnhanceWeapon()
   {
      foreach (Weapon weapon in _gameManager.Player.WeaponList)
      {
         float modifiedValue = weapon.DamageModifier * _weaponEnhancerUpgrade.WeaponEnhancerUpgradeDetails.AttackModifier;
         weapon.ChangeDamageModifier(modifiedValue);
      }
   }
}
