using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Enums;

/// <summary>
/// Manages player input using the Unity Input System.
/// Dispatches events via EventCenter to decouple input from game logic.
/// </summary>
[RequireComponent(typeof(Player), typeof(PlayerInput))]
public class InputReceiver : MonoBehaviour
{
    #region === Fields ===

    private Player _player;
    private PlayerInput _playerInput;

    private InputAction _rotateAction;

    #endregion
    
    [SerializeField] private bool printLogs = false;

    #region === Unity Lifecycle ===

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        UnregisterCallbacks();
    }

    #endregion

    #region === Initialization ===

    private void Init()
    {
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();

        _rotateAction = _playerInput.actions["Rotate"];
        _rotateAction.performed += OnRotatePerformed;
        _rotateAction.canceled += OnRotateCanceled;
    }

    private void UnregisterCallbacks()
    {
        if (_rotateAction != null)
        {
            _rotateAction.performed -= OnRotatePerformed;
            _rotateAction.canceled -= OnRotateCanceled;
        }
    }

    #endregion

    #region === Arm Controls ===

    public void Gameplay_NewExtendDown(InputAction.CallbackContext context) => HandleArmInput(context, 0);
    public void Gameplay_NewExtendLeft(InputAction.CallbackContext context) => HandleArmInput(context, 1);
    public void Gameplay_NewExtendUp(InputAction.CallbackContext context) => HandleArmInput(context, 2);
    public void Gameplay_NewExtendRight(InputAction.CallbackContext context) => HandleArmInput(context, 3);

    private void HandleArmInput(InputAction.CallbackContext context, int direction)
    {
        if (context.started)
        {
            _player.EventCenter.Invoke<int>(PlayerEvent.OnHoldArm, direction);
        }

        if (context.canceled || context.interaction is TapInteraction)
        {
            _player.EventCenter.Invoke<int>(PlayerEvent.OnExtendArm, direction);
        }
    }

    #endregion

    #region === Rotation ===

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        _player.EventCenter.Invoke<float>(PlayerEvent.OnRotate, value);
    }

    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        _player.EventCenter.Invoke<float>(PlayerEvent.OnRotate, 0f);
    }

    #endregion

    #region === Gameplay Controls ===

    public void Gameplay_Start(InputAction.CallbackContext context)
    {
        if (!context.canceled) return;

        var state = GameManager.Instance.GlobalGameState;

        switch (state)
        {
            case GlobalGameState.Outro:
                GEventCenter.Invoke(GameEvent.OnGameReset);
                break;

            case GlobalGameState.ScoreScreen:
	            GEventCenter.Invoke(GameManager.Instance.PlayerHasWon
                    ? (GameEvent.OnLoadOutro)
                    : GameEvent.OnStartRound);
                break;

            case GlobalGameState.WinnerScreen:
	            GEventCenter.Invoke(GameEvent.OnShowScore);
                break;

            case GlobalGameState.InPlay:
	            if (LevelManager.Instance.IsLobbyScene()) _player.EventCenter.Invoke(PlayerEvent.OnPlayerReady);
	            else GEventCenter.Invoke(GameEvent.OnPauseGame);
                break;
        }
    }

    public void Gameplay_NextCharacter(InputAction.CallbackContext context)
    {
        if (IsLobbyScene() && context.interaction is PressInteraction && context.started)
        {
            _player.EventCenter.Invoke(PlayerEvent.OnNextCharacter);
        }
    }

    public void Gameplay_PreviousCharacter(InputAction.CallbackContext context)
    {
        if (IsLobbyScene() && context.interaction is PressInteraction && context.started)
        {
            _player.EventCenter.Invoke(PlayerEvent.OnPreviousCharacter);
        }
    }

    public void Gameplay_UseItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _player.EventCenter.Invoke(PlayerEvent.OnUseItem);
        }
    }

    public void Gameplay_Suicide(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        if (context.started && !_player.IsDead())
        {
            _player.EventCenter.Invoke(PlayerEvent.OnKillSelf);
        }
#endif
    }

    public void Debug_NewScene(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _player.EventCenter.Invoke(PlayerEvent.NewRound);
        }
    }

    private bool IsLobbyScene() => LevelManager.Instance.CurrentSceneIndex == 2;

    #endregion

    #region === Menu Controls ===

    public void Menu_GoUp(InputAction.CallbackContext context)
    {
        if (IsMenuInput(context))
        {
            LogPlayerControls("Menu_GoUp");
	        GEventCenter.Invoke(GameEvent.OnMenuUp);
        }
    }

    public void Menu_GoDown(InputAction.CallbackContext context)
    {
        if (IsMenuInput(context))
        {
	        LogPlayerControls("Menu_GoDown");
	        GEventCenter.Invoke(GameEvent.OnMenuDown);
        }
    }

    public void Menu_GoLeft(InputAction.CallbackContext context)
    {
        if (IsMenuInput(context))
        {
	        GEventCenter.Invoke(GameEvent.OnMenuLeft);
        }
    }

    public void Menu_GoRight(InputAction.CallbackContext context)
    {
        if (IsMenuInput(context))
        {
	        GEventCenter.Invoke(GameEvent.OnMenuRight);
        }
    }

    public void Menu_Validate(InputAction.CallbackContext context)
    {
        if (IsMenuInput(context))
        {
	        GEventCenter.Invoke(GameEvent.OnMenuValidate);
        }
    }

    private bool IsMenuInput(InputAction.CallbackContext context)
    {
        return gameObject.scene.IsValid()
               && context.interaction is PressInteraction
               && context.canceled;
    }

    #endregion
    
    #region Utils
    private void LogPlayerControls(string action)
	{
		if (printLogs) Debug.Log($"Player {_player.PlayerData.PlayerIndex + 1} is pressing {action}");
	}
	
    #endregion
    
}
