using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Vector2 _movement;
    private Player _player;
    private VampireStuffInputActions _vampireStuffInputActions;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _vampireStuffInputActions = new VampireStuffInputActions();
    }

    private void OnEnable()
    {
        EnablePlayerControls(true);

        _vampireStuffInputActions.Player.Move.performed += OnPlayerMovePerformed;
        _vampireStuffInputActions.Player.Move.canceled += OnPlayerMoveCanceled;
        _vampireStuffInputActions.Player.Use.performed += OnPlayerUsePerformed;
        _vampireStuffInputActions.Player.Detonate.performed += OnPlayerDetonatePerformed;

        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        EnablePlayerControls(false);

        _vampireStuffInputActions.Player.Move.performed -= OnPlayerMovePerformed;
        _vampireStuffInputActions.Player.Move.canceled -= OnPlayerMoveCanceled;
        _vampireStuffInputActions.Player.Use.performed -= OnPlayerUsePerformed;
        _vampireStuffInputActions.Player.Detonate.performed -= OnPlayerDetonatePerformed;

        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        if (args.GameState == GameState.PauseMenu)
        {
            EnablePlayerControls(false);
        }
        else if (args.GameState == GameState.Play)
        {
            EnablePlayerControls(true);
        }
    }

    private void Update()
    {
        if (!_vampireStuffInputActions.Player.enabled)
        {
            _movement = Vector2.zero;
        }

        MovementInput();
    }

    private void OnPlayerMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();

        // TODO movement tweak needed
        if (movementInput.x != 0f && movementInput.y != 0f)
        {
            movementInput *= 0.7f;
        }

        _movement = movementInput;
    }

    private void OnPlayerMoveCanceled(InputAction.CallbackContext context)
    {
        _movement = Vector2.zero;
    }

    private void OnPlayerUsePerformed(InputAction.CallbackContext context)
    {
        UseItemInput();
    }
    
    private void OnPlayerDetonatePerformed(InputAction.CallbackContext context)
    {
        if (!_player.BombOperator.HasBombInPocket)
        {
            return;
        }

        _player.BombOperator.OperateBomb();
    }

    private void MovementInput()
    {
        _player.MovementToVelocityEvent.CallMovementToVelocityEvent(_player.PlayerDetails.Speed, _movement);

        if (_movement == Vector2.zero)
        {
            _player.IdleEvent.CallIdleEvent();
        }
    }

    private void UseItemInput()
    {
        float useItemRadius = 2f;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(_player.GetPlayerPosition(), useItemRadius);

        foreach (Collider2D item in collider2DArray)
        {
            IUsable iUsable = item.GetComponent<IUsable>();

            if (iUsable != null)
            {
                iUsable.UseItem();
            }
        }
    }

    private void EnablePlayerControls(bool shouldEnable)
    {
        if (shouldEnable)
        {
            if (_vampireStuffInputActions.Player.enabled)
            {
                return;
            }

            _vampireStuffInputActions.Player.Enable();
        }
        else
        {
            if (!_vampireStuffInputActions.Player.enabled)
            {   
                return; 
            }

            _vampireStuffInputActions.Player.Disable();
        }
    }
}
