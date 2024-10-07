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
    [SerializeField] private Image _speedEnhancerSlotImage;
    [Header("Game Object Text References")]
    [SerializeField] private TMP_Text _bombSlotTitleText;
    [SerializeField] private TMP_Text _speedEnhancerTitleText;
    [SerializeField] private TMP_Text _bombSlotSpecsText;
    [SerializeField] private TMP_Text _speedEnhancerSpecsText;

    private GameObject _bombSlotGameObject;
    private GameObject _speedEnhancerSlotGameObject;
    private Image _weaponEnhancerSlotImage;
    private GameManager _gameManager => GameManager.Instance;
    private UpgradeSpawner _upgradeSpawner => UpgradeSpawner.Instance;
    
    private void OnEnable()
    {
        // Bomb Slot
        _bombSlotGameObject = _upgradeSpawner.FillBombSlot();
        _speedEnhancerSlotGameObject = _upgradeSpawner.FillSpeedEnhancerSlot();
        
        _bombSlotImage.sprite = _bombSlotGameObject.GetComponent<BombUpgrade>().BombUpgradeDetails.Sprite;
        _bombSlotImage.color = _bombSlotGameObject.GetComponent<BombUpgrade>().BombUpgradeDetails.SpriteColor;

        _speedEnhancerSlotImage.sprite =
            _speedEnhancerSlotGameObject.GetComponent<SpeedEnhancerUpgrade>().SpeedEnhancerUpgradeDetails.Sprite;
        _speedEnhancerSlotImage.color =
            _speedEnhancerSlotGameObject.GetComponent<SpeedEnhancerUpgrade>().SpeedEnhancerUpgradeDetails.SpriteColor;

        // TODO: Change the code with upgrade details
        _bombSlotTitleText.text = "Bomb";
        _speedEnhancerTitleText.text = "Speed Enhancer";
    }

    public void SelectBombUpgrade()
    {
        _gameManager.Player.BombOperator.AddBombToPocket(_bombSlotGameObject);
        StaticEventHandler.CallGameStateChangedEvent(GameState.Play);
    }

    public void SelectSpeedEnhancerUpgrade()
    {
        _upgradeSpawner.SpawnSpeedEnhancer(_speedEnhancerSlotGameObject);
        StaticEventHandler.CallGameStateChangedEvent(GameState.Play);
    }
}
