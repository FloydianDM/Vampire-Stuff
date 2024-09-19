using TMPro;
using UnityEngine;

public class PlayerLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerLevelText;

    private GameManager _gameManager => GameManager.Instance;

    private void OnEnable()
    {
        _gameManager.Player.LevelUpEvent.OnLevelUpEvent += LevelUpEvent_OnLevelUpEvent;
        _playerLevelText.text = "LEVEL " + _gameManager.Player.PlayerLevelManager.PlayerLevel;
    }

    private void OnDisable()
    {
        _gameManager.Player.LevelUpEvent.OnLevelUpEvent -= LevelUpEvent_OnLevelUpEvent;
    }

    private void LevelUpEvent_OnLevelUpEvent(LevelUpEvent @event, LevelUpEventArgs args)
    {
        _playerLevelText.text = "LEVEL " + args.PlayerLevel;
    }
}
