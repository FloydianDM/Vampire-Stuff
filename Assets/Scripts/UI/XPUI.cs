using UnityEngine;
using UnityEngine.UI;

public class XPUI : MonoBehaviour
{
    [SerializeField] private Image _xpUIImage;

    private GameManager _gameManager => GameManager.Instance;

    private void Start()
    {
        _xpUIImage.fillAmount = 0;
    }

    private void OnEnable()
    {
        _gameManager.Player.ReceiveXPEvent.OnReceiveXPEvent += ReceiveXPEvent_OnReceiveXPEvent;
    }

    private void OnDisable()
    {
        _gameManager.Player.ReceiveXPEvent.OnReceiveXPEvent -= ReceiveXPEvent_OnReceiveXPEvent;
    }

    private void ReceiveXPEvent_OnReceiveXPEvent(ReceiveXPEvent @event, ReceiveXPEventArgs args)
    {
        float fillAmount = (float)_gameManager.Player.ReceiveXP.XP / _gameManager.Player.ReceiveXP.XPThreshold;
        _xpUIImage.fillAmount = fillAmount;
    }
}
