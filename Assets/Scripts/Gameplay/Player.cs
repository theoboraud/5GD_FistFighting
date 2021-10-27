using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using Enums;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class Player : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public Rigidbody2D RB;       // Player rigidbody ref
    public ArmBehaviour[] Arms = new ArmBehaviour[4];     // Array containing each arm
    [System.NonSerialized] public CharacterSkin CharSkin;
    [SerializeField] private SpriteRenderer SpriteFace;
    [SerializeField] private SpriteRenderer[] SpriteArms;
    [System.NonSerialized] public PlayerSelector PlayerSelector;

    [Header("Events for FMOD")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [Header("Controls")]
    private PlayerInput playerInput;
    private GameplayControls gameplayControls;
    private MenuControls menuControls;

    [Header("Variables")]
    [System.NonSerialized] public PlayerState PlayerState;
    // #endregion


    // #region ===================== INIT FUNCTIONS ====================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        InitReferences();
        InitControls();
        InitVariables();

        if (PlayersManager.Instance != null)
        {
            PlayersManager.Instance.AddPlayer(this);
            CharSkin = PlayersManager.Instance.SkinsData.GetRandomSkin();
            InitSkin();
        }
    }


    private void InitReferences()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
    }


    private void InitControls()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        gameplayControls = gameObject.GetComponent<GameplayControls>();
        menuControls = gameObject.GetComponent<MenuControls>();
    }


    private void InitVariables()
    {
        PlayerState = PlayerState.NotReady;
    }


    private void InitSkin()
    {
        SpriteFace.sprite = CharSkin.SpriteFace;
        for (int i = 0; i < SpriteArms.Length; i++)
        {
            SpriteArms[i].sprite = CharSkin.SpriteArm;
        }
    }

    // #endregion


    // #region ===================== SKIN FUNCTIONS ====================

    public void ChangeSkin(CharacterSkin _charSkin)
    {
        this.CharSkin = _charSkin;
        InitSkin();
    }

    // #endregion


    // #region ============== GAMEPLAY CONTROLS FUNCTIONS ==============

    // TO REIMPLEMENT

    // #endregion


    // #region ================ MENU CONTROLS FUNCTIONS ================

    public void Menu_GoUp(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GameState == GameState.CharacterSelectMenu)
            {

            }
            else
            {
                menuControls.GoUp();
            }
        }
    }

    public void Menu_GoRight(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GameState == GameState.CharacterSelectMenu)
            {
                PlayerSelector.ChangeSkinRight();
            }
            else
            {
                menuControls.GoRight();
            }
        }
    }

    public void Menu_GoDown(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GameState == GameState.CharacterSelectMenu)
            {

            }
            else
            {
                menuControls.GoDown();
            }
        }
    }

    public void Menu_GoLeft(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GameState == GameState.CharacterSelectMenu)
            {
                PlayerSelector.ChangeSkinLeft();
            }
            else
            {
                menuControls.GoLeft();
            }
        }
    }

    public void Menu_Validate(InputAction.CallbackContext _context)
    {
        if (playerInput.currentActionMap.name == "Menu" && _context.started && _context.interaction is PressInteraction)
        {
            if (GameManager.Instance.GameState == GameState.CharacterSelectMenu)
            {
                PlayerSelector.ValidateSkin();
            }
            else
            {
                menuControls.Validate();
            }
        }
    }

    // #endregion
}
