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
    public Player Player;                      // Player reference
    private List<ArmBehaviour> arms;            // Player arms references
    public RotateBehaviour Rotate;             // Player rotate reference
    private PlayerInput playerInput;            // Player Input reference

    // #endregion



    // #region ============== INIT FUNCTIONS ==============

    /// <summary>
    ///     Init references
    /// </summary>
    public void Init()
    {
        arms = new List<ArmBehaviour>();
        foreach (ArmBehaviour _arm in Player.Arms)
        {
            arms.Add(_arm);
        }

        Rotate = gameObject.GetComponent<RotateBehaviour>();

        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    // #endregion



    // #region ============== GAMEPLAY CONTROLS FUNCTIONS ==============

    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendUp(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Player.Arms[0].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when extending right arm
    /// </summary>
    public void Gameplay_ExtendRight(InputAction.CallbackContext _context)
    {
        print(PlayersManager.Instance.Players.Count);
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Player.Arms[1].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendDown(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Player.Arms[2].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendLeft(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            Player.Arms[3].Input_Extend(_context);
        }
    }


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
    }


    /// <summary>
    ///     Called when pressing start to end the round
    /// </summary>
    public void Gameplay_Start(InputAction.CallbackContext _context)
    {
        if (GameManager.Instance.GlobalGameState == GlobalGameState.ScoreScreen)
        {
            GameManager.Instance.NewGameRound();
        }
    }


    /// <summary>
    ///     Called when using the P key for ending the round (as if someone has won and pressed Start)
    /// </summary>
    public void Debug_NewScene(InputAction.CallbackContext _context)
    {
        GameManager.Instance.NewGameRound();
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
