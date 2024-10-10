using TMPro;
using UnityEngine;

public class BombNotifierUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bombNotifierText;

    private GameManager _gameManager => GameManager.Instance;

    private void Awake()
    {
        HideBombNotifier(); 
    }

    private void OnEnable()
    {
        StaticEventHandler.OnBombReady += StaticEventHandler_OnBombReady;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnBombReady -= StaticEventHandler_OnBombReady;
    }

    private void StaticEventHandler_OnBombReady(BombReadyEventArgs args)
    {
        if (args.IsBombReady)
        {
            ShowBombNotifier();
        }
        else
        {
            HideBombNotifier();
        }
    }

    private void ShowBombNotifier()
    {
        _bombNotifierText.gameObject.SetActive(true);

        _bombNotifierText.text = 
            _gameManager.Player.BombOperator.BombPocket.GetComponent<BombUpgrade>().BombUpgradeDetails.Type + " Bomb is Ready!";
    }
    
    private void HideBombNotifier()
    {
        _bombNotifierText.gameObject.SetActive(false);
    }
}
