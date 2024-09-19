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
        if (_gameManager.GameState == GameState.Play)
        {
            _gameManager.GameState = GameState.PauseMenu;

            StaticEventHandler.CallGameStateChangedEvent(_gameManager.GameState);
        }
        else if (_gameManager.GameState == GameState.PauseMenu)
        {
            _gameManager.GameState = GameState.Play;

            StaticEventHandler.CallGameStateChangedEvent(_gameManager.GameState);
        }  
    }
}
