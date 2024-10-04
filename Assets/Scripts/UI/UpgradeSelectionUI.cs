using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _bombSlot;
    [SerializeField] private GameObject _weaponEnhancerSlot;
    [SerializeField] private GameObject _speedEnhancerSlot;
    [SerializeField] private Image _bombSlotImage;

    private GameObject _bombSlotGameObject;
    private Image _weaponEnhancerSlotImage;
    private Image _speedEnhancerSlotImage;
    private GameManager _gameManager => GameManager.Instance;
    private UpgradeSpawner _upgradeSpawner => UpgradeSpawner.Instance;
    
    private void OnEnable()
    {
        // Bomb Slot
        _bombSlotGameObject = _upgradeSpawner.FillBombSlot();
        _bombSlotImage.sprite = _bombSlotGameObject.GetComponent<BombUpgrade>().BombUpgradeDetails.Sprite;
        _bombSlotImage.color = _bombSlotGameObject.GetComponent<BombUpgrade>().BombUpgradeDetails.SpriteColor;
    }

    public void SelectBombUpgrade()
    {
        _gameManager.Player.BombOperator.AddBombToPocket(_bombSlotGameObject);
        StaticEventHandler.CallGameStateChangedEvent(GameState.Play);
    }
}
