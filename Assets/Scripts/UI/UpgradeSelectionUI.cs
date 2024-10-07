using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionUI : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField] private GameObject _bombSlot;
    [SerializeField] private GameObject _weaponEnhancerSlot;
    [SerializeField] private GameObject _speedEnhancerSlot;
    [Header("Game Object Image References")]
    [SerializeField] private Image _bombSlotImage;
    [SerializeField] private Image _weaponEnhancerSlotImage;
    [SerializeField] private Image _speedEnhancerSlotImage;
    [Header("Game Object Text References")]
    [SerializeField] private TMP_Text _bombSlotTitleText;
    [SerializeField] private TMP_Text _weaponEnhancerTitleText;
    [SerializeField] private TMP_Text _speedEnhancerTitleText;
    [SerializeField] private TMP_Text _bombSlotSpecsText;
    [SerializeField] private TMP_Text _weaponEnhancerSpecsText;
    [SerializeField] private TMP_Text _speedEnhancerSpecsText;

    private GameObject _bombSlotGameObject;
    private GameObject _weaponEnhancerSlotGameObject;
    private GameObject _speedEnhancerSlotGameObject;
    private GameManager _gameManager => GameManager.Instance;
    private UpgradeSpawner _upgradeSpawner => UpgradeSpawner.Instance;
    
    private void OnEnable()
    {
        // Bomb Slot
        _bombSlotGameObject = _upgradeSpawner.FillBombSlot();
        BombUpgrade bombUpgrade = _bombSlotGameObject.GetComponent<BombUpgrade>();
        
        _bombSlotImage.sprite = bombUpgrade.BombUpgradeDetails.Sprite;
        _bombSlotImage.color = bombUpgrade.BombUpgradeDetails.SpriteColor;

        // Weapon Enhancer Slot
        _weaponEnhancerSlotGameObject = _upgradeSpawner.FillWeaponEnhancerSlot();
        WeaponEnhancerUpgrade weaponEnhancerUpgrade = _weaponEnhancerSlotGameObject.GetComponent<WeaponEnhancerUpgrade>();
        
        _weaponEnhancerSlotImage.sprite = weaponEnhancerUpgrade.WeaponEnhancerUpgradeDetails.Sprite;
        _weaponEnhancerSlotImage.color = weaponEnhancerUpgrade.WeaponEnhancerUpgradeDetails.SpriteColor;

        // Speed Enhancer Slot
        _speedEnhancerSlotGameObject = _upgradeSpawner.FillSpeedEnhancerSlot();
        SpeedEnhancerUpgrade speedEnhancerUpgrade = _speedEnhancerSlotGameObject.GetComponent<SpeedEnhancerUpgrade>();
        
        _speedEnhancerSlotImage.sprite = speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails.Sprite;
        _speedEnhancerSlotImage.color = speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails.SpriteColor;
        
        // Slot Type Texts
        _bombSlotTitleText.text = "Bomb - " + bombUpgrade.BombUpgradeDetails.Type;
        _weaponEnhancerTitleText.text = "Weapon Enhancer - " + weaponEnhancerUpgrade.WeaponEnhancerUpgradeDetails.Type;
        _speedEnhancerTitleText.text = "Speed Enhancer - " + speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails.Type;
        
        // Slot Spec Texts
        _bombSlotSpecsText.text = "Damage - " + bombUpgrade.BombUpgradeDetails.Damage + "\n" +
                                  "Impact Area - " + bombUpgrade.BombUpgradeDetails.ImpactArea + "\n" +
                                  "Cooldown Time - " + bombUpgrade.BombUpgradeDetails.CooldownTime;
        _weaponEnhancerSpecsText.text = "Attack Modifier - " + weaponEnhancerUpgrade.WeaponEnhancerUpgradeDetails.AttackModifier + " x";
        _speedEnhancerSpecsText.text = "Speed Modifier - " + speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails.SpeedModifier + " x";
    }

    public void SelectBombUpgrade()
    {
        _gameManager.Player.BombOperator.AddBombToPocket(_bombSlotGameObject);
        StaticEventHandler.CallGameStateChangedEvent(GameState.Play);
    }

    public void SelectWeaponEnhancerUpgrade()
    {
        _upgradeSpawner.SpawnWeaponEnhancer(_weaponEnhancerSlotGameObject);
        StaticEventHandler.CallGameStateChangedEvent(GameState.Play);
    }

    public void SelectSpeedEnhancerUpgrade()
    {
        _upgradeSpawner.SpawnSpeedEnhancer(_speedEnhancerSlotGameObject);
        StaticEventHandler.CallGameStateChangedEvent(GameState.Play);
    }
}
