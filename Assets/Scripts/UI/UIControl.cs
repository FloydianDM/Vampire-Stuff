using UnityEngine;
using UnityEngine.InputSystem;

public class UIControl : MonoBehaviour
{
    private VampireStuffInputActions _vampireStuffInputActions;

    private GameManager _gameManager => GameManager.Instance;

    private void OnEnable()
    {
        _vampireStuffInputActions = new VampireStuffInputActions();
        _vampireStuffInputActions.Enable();

        _vampireStuffInputActions.UI.Pause.performed += OnPauseClicked;
    }

    private void OnDisable()
    {
        _vampireStuffInputActions.UI.Pause.performed -= OnPauseClicked;
    }

    private void OnPauseClicked(InputAction.CallbackContext context)
    {
        switch (_gameManager.GameState)
        {
            case GameState.Play:
                _gameManager.GameState = GameState.Pause;

                StaticEventHandler.CallGameStateChangedEvent(_gameManager.GameState);
                break;
            case GameState.Pause:
                _gameManager.GameState = GameState.Play;

                StaticEventHandler.CallGameStateChangedEvent(_gameManager.GameState);
                break;
        }
    }
}
