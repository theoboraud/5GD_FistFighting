using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Enums;

/// <summary>
///     Class used for player controls
/// </summary>
public class PlayerControls : MonoBehaviour
{
    // #region ============== CLASS VARIABLES ==============

    [Header("References")]
    public Player Player;                       // Player reference
    private List<ArmBehaviour> arms;            // Player arms references
    private RotateBehaviour Rotate;              // Player rotate reference
    private PlayerInput playerInput;            // Player Input reference
    // All input action references
    private InputAction action_rotate;

    // #endregion



    // #region ============== INIT FUNCTIONS ==============

    /// <summary>
    ///     Init references
    /// </summary>
    public void Init()
    {
        Player = gameObject.GetComponent<Player>();

        arms = new List<ArmBehaviour>();
        foreach (ArmBehaviour _arm in Player.Arms)
        {
            arms.Add(_arm);
        }

        Rotate = gameObject.GetComponent<RotateBehaviour>();

        playerInput = gameObject.GetComponent<PlayerInput>();

        // Assign each action to corresponding inputs
        //action_upArm = playerInput.actions["UpArm"];
        //action_rightArm = playerInput.actions["RightArm"];
        //action_downArm = playerInput.actions["DownArm"];
        //action_leftArm = playerInput.actions["LeftArm"];
        action_rotate = playerInput.actions["Rotate"];
        //action_start = playerInput.actions["Start"];
        //action_holdTrigger = playerInput.actions["HoldTrigger"];
    }

    // #endregion



    // #region ============== GAMEPLAY CONTROLS FUNCTIONS ==============

    public void Gameplay_NewExtendDown(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            Player.PlayerArmController.HoldArm(0);
        }
        if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.PlayerArmController.ExtendArm(0);
        }
    }

    public void Gameplay_NewExtendLeft(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            Player.PlayerArmController.HoldArm(1);
        }
        if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.PlayerArmController.ExtendArm(1);
        }
    }

    public void Gameplay_NewExtendUp(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            Player.PlayerArmController.HoldArm(2);
        }
        if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.PlayerArmController.ExtendArm(2);
        }
    }

    public void Gameplay_NewExtendRight(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            Player.PlayerArmController.HoldArm(3);
        }
        if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.PlayerArmController.ExtendArm(3);
        }
    }

    /// <summary>
    ///     Called when rotating using the stick
    /// </summary>
    public void Gameplay_RotateStick(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Player.Arms[3].Input_StopExtend();
        }
    }


    /// <summary>
    ///     Called when rotating using the stick
    /// </summary>
    public void Gameplay_Rotate(InputAction.CallbackContext _context)
    {
        //Rotate.Input_Rotating(_context.ReadValue<float>());
    }


    /// <summary>
    ///     Called for checking sticks value
    /// </summary>
    private void Update()
    {
        Rotate.Input_Rotating(action_rotate.ReadValue<float>());
    }


    /// <summary>
    ///     Called when pressing start to end the round
    /// </summary>
    public void Gameplay_Start(InputAction.CallbackContext _context)
    {
        if (GameManager.Instance.GlobalGameState == GlobalGameState.PlayerWon && _context.canceled)
        {
            GameManager.Instance.ResetGame();
        }

        else if (GameManager.Instance.GlobalGameState == GlobalGameState.ScoreScreen && _context.canceled)
        {
            GameManager.Instance.NewGameRound();
        }

        else if (GameManager.Instance.GlobalGameState == GlobalGameState.WinnerScreen && _context.canceled)
        {
            GameManager.Instance.ScoreScreen();
        }
    }


    /// <summary>
    ///     Called when using controls for changing characters in the lobby
    /// </summary>
    public void Gameplay_NextCharacter(InputAction.CallbackContext _context)
    {
        if (LevelManager.Instance.CurrentSceneIndex == 0 && _context.interaction is PressInteraction && _context.started)
        {
            Player.NextCharacter();
        }
    }


    /// <summary>
    ///     Called when using controls for changing characters in the lobby
    /// </summary>
    public void Gameplay_PreviousCharacter(InputAction.CallbackContext _context)
    {
        if (LevelManager.Instance.CurrentSceneIndex == 0 && _context.interaction is PressInteraction && _context.started)
        {
            Player.PreviousCharacter();
        }
    }


    /// <summary>
    ///     Called when using the P key for ending the round (as if someone has won and pressed Start)
    /// </summary>
    public void Debug_NewScene(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            GameManager.Instance.NewGameRound();
        }
    }


    /// <summary>
    ///     Called when using the player holds any of the Triggers of the controller
    /// </summary>
    public void Gameplay_UseItem(InputAction.CallbackContext _context)
    {
        // TODO: add HoldingItem condition
        if (_context.started)
        {
            // TODO: Add item usage
        }
    }


    public void Gameplay_Suicide(InputAction.CallbackContext _context)
    {
        // Allows players to kill themselves if testing in the editor
        #if UNITY_EDITOR
        if (_context.started && Player.PlayerGameState is PlayerGameState.Alive)
        {
            Player.Kill();
        }
        #endif
    }

    // #endregion



    // #region ================ MENU CONTROLS FUNCTIONS ================

    public void Menu_GoUp(InputAction.CallbackContext _context)
    {
        if (gameObject.scene.IsValid() && _context.interaction is PressInteraction && _context.canceled)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.MainMenu)
            {
                MenuManager.Instance.MainMenu.GoUp();
            }
        }
    }


    public void Menu_GoDown(InputAction.CallbackContext _context)
    {
        if (gameObject.scene.IsValid() && _context.interaction is PressInteraction && _context.canceled)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.MainMenu)
            {
                MenuManager.Instance.MainMenu.GoDown();
            }
        }
    }


    public void Menu_GoRight(InputAction.CallbackContext _context)
    {

    }


    public void Menu_GoLeft(InputAction.CallbackContext _context)
    {

    }


    public void Menu_Validate(InputAction.CallbackContext _context)
    {
        if (gameObject.scene.IsValid() && _context.interaction is PressInteraction && _context.canceled)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.MainMenu)
            {
                MenuManager.Instance.MainMenu.Validate();
            }
        }
    }

    // #endregion
}
