using UnityEngine;

[DisallowMultipleComponent]
public class Weapon : MonoBehaviour
{
    public WeaponDetailsSO WeaponDetails;
    public int WeaponListPosition;
    public Transform ShootPosition;
    public float DamageModifier { get; private set; } = 1f;

    private GameManager _gameManager => GameManager.Instance;

    private void Awake()
    {
        if (!_gameManager.Player.IsWeaponHeldByPlayer(WeaponDetails))
        {
            _gameManager.Player.AddWeaponToPlayerWeaponList(WeaponDetails);
            Debug.Log("Weapon has added to weapon list");
        }
    }

    public void ChangeDamageModifier(float modifiedValue)
    {
        DamageModifier = modifiedValue;
    }
}
