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
    private Player player;                      // Player reference
    private List<ArmBehaviour> arms;            // Player arms references
    private RotateBehaviour rotate;             // Player rotate reference
    private PlayerInput playerInput;            // Player Input reference

    // #endregion



    // #region ============== UNITY FUNCTIONS ==============

    /// <summary>
    ///     Init references
    /// </summary>
    private void Awake()
    {
        player = gameObject.GetComponent<Player>();
        
        arms = new List<ArmBehaviour>();
        foreach (ArmBehaviour _arm in player.Arms)
        {
            arms.Add(_arm);
        }

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
        if (player.PlayerGameState != PlayerGameState.Dead)
        {
            player.Arms[0].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when extending right arm
    /// </summary>
    public void Gameplay_ExtendRight(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (player.PlayerGameState != PlayerGameState.Dead)
        {
            player.Arms[1].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendDown(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (player.PlayerGameState != PlayerGameState.Dead)
        {
            player.Arms[2].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when extending up arm
    /// </summary>
    public void Gameplay_ExtendLeft(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (player.PlayerGameState != PlayerGameState.Dead)
        {
            player.Arms[3].Input_Extend(_context);
        }
    }


    /// <summary>
    ///     Called when rotating right
    /// </summary>
    public void Gameplay_RotateRight(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (player.PlayerGameState != PlayerGameState.Dead)
        {
            rotate.Input_RotateRight(_context);
        }
    }


    /// <summary>
    ///     Called when rotating left
    /// </summary>
    public void Gameplay_RotateLeft(InputAction.CallbackContext _context)
    {
        // If the player is not dead
        if (player.PlayerGameState != PlayerGameState.Dead)
        {
            rotate.Input_RotateLeft(_context);
        }
    }


    public void Gameplay_Start(InputAction.CallbackContext _context)
    {
        if (GameManager.Instance.GlobalGameState == GlobalGameState.ScoreScreen)
        {
            GameManager.Instance.NewGameRound();
        }
    }


    public void Debug_NewScene(InputAction.CallbackContext _context)
    {
        GameManager.Instance.NewGameRound();
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
