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

    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendUp(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (_context.started)
        {
            Player.Arms[0].Input_StartExtend();
        }
        else if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.Arms[0].Input_StopExtend();
        }
    }


    /// <summary>
    ///     Called when extending right arm
    /// </summary>
    public void Gameplay_ExtendRight(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (_context.started)
        {
            Player.Arms[1].Input_StartExtend();
        }
        else if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.Arms[1].Input_StopExtend();
        }
    }


    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendDown(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (_context.started)
        {
            Player.Arms[2].Input_StartExtend();
        }
        if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.Arms[2].Input_StopExtend();
        }
    }


    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendLeft(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (_context.started)
        {
            Player.Arms[3].Input_StartExtend();
        }
        else if (_context.canceled || _context.interaction is TapInteraction)
        {
            Player.Arms[3].Input_StopExtend();
        }
    }

    public void Gameplay_NewExtendDown(InputAction.CallbackContext _context)
    {
        if(_context.control.IsPressed())
        {
            Player.PlayerArmController.HoldArm(0);
        }
        else
        {
            Player.PlayerArmController.ExtendArm(0);
        }
    }

    public void Gameplay_NewExtendLeft(InputAction.CallbackContext _context)
    {
        if (_context.control.IsPressed())
        {
            Player.PlayerArmController.HoldArm(1);
        }
        else
        {
            Player.PlayerArmController.ExtendArm(1);
        }
    }

    public void Gameplay_NewExtendUp(InputAction.CallbackContext _context)
    {
        if (_context.control.IsPressed())
        {
            Player.PlayerArmController.HoldArm(2);
        }
        else
        {
            Player.PlayerArmController.ExtendArm(2);
        }
    }

    public void Gameplay_NewExtendRight(InputAction.CallbackContext _context)
    {
        if (_context.control.IsPressed())
        {
            Player.PlayerArmController.HoldArm(3);
        }
        else
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


    private void Update()
    {
        Rotate.Input_Rotating(action_rotate.ReadValue<float>());
    }


    /*
    /// <summary>
    ///     Called when rotating right
    /// </summary>
    public void Gameplay_RotateRight(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Rotate.Input_RotateRight(_context);
        }
    }


    /// <summary>
    ///     Called when rotating left
    /// </summary>
    public void Gameplay_RotateLeft(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Rotate.Input_RotateLeft(_context);
        }
    }*/


    /// <summary>
    ///     Called when pressing start to end the round
    /// </summary>
    public void Gameplay_Start(InputAction.CallbackContext _context)
    {
        if (GameManager.Instance.GlobalGameState == GlobalGameState.ScoreScreen && _context.started)
        {
            GameManager.Instance.NewGameRound();
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
    public void HoldTrigger(InputAction.CallbackContext _context)
    {
        Player.HoldingTrigger = true;
        if(!_context.control.IsPressed())
        {
            Player.HoldingTrigger = false;
        }
    }

    // #endregion



    // #region ================ MENU CONTROLS FUNCTIONS ================

    public void Menu_GoUp(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.CharacterSelectMenu)
            {

            }
            else
            {

            }
        }
    }

    public void Menu_GoRight(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.CharacterSelectMenu)
            {
                //PlayerSelector.ChangeSkinRight();
            }
            else
            {

            }
        }
    }

    public void Menu_GoDown(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.CharacterSelectMenu)
            {

            }
            else
            {

            }
        }
    }

    public void Menu_GoLeft(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.CharacterSelectMenu)
            {
                //PlayerSelector.ChangeSkinLeft();
            }
            else
            {

            }
        }
    }

    public void Menu_Validate(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GlobalGameState == GlobalGameState.CharacterSelectMenu)
            {
                //PlayerSelector.ValidateSkin();
            }
            else
            {

            }
        }
    }

    // #endregion
}
