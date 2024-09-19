using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _healthUI;
    [SerializeField] private GameObject _xpUI;
    [SerializeField] private GameObject _playerLevelUI;

    private void Start()
    {
        ControlUI(GameState.Play);
    }

    private void OnEnable()
    {
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        ControlUI(args.GameState);
    }

    private void ControlUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
                _healthUI.SetActive(true);
                _xpUI.SetActive(true);
                _playerLevelUI.SetActive(true);
                break;
            case GameState.PauseMenu:
                _healthUI.SetActive(false);
                _xpUI.SetActive(false);
                _playerLevelUI.SetActive(false);
                break;
        }
    }
}
