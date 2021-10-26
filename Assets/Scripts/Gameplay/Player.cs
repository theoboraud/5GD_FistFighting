using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class Player : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public Rigidbody2D RB;       // Player rigidbody ref
    public ArmBehaviour[] Arms = new ArmBehaviour[4];     // Array containing each arm
    public CharacterSkin CharSkin;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private SpriteRenderer[] Arm;
    public SkinSelector SkinSelector;
    private PlayerInput playerInput;

    [Header("Events")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [Header("MenuElements")]
    [System.NonSerialized] public MenuTravel MenuTravel;
    // #endregion


    // #region ================ INIT CONTROLS FUNCTIONS ================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        MenuTravel = FindObjectOfType<MenuTravel>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        RB = gameObject.GetComponent<Rigidbody2D>();
        if (GameManager.Singleton_GameManager != null)
        {
            GameManager.Singleton_GameManager.AddPlayer(this);
            this.transform.parent = GameManager.Singleton_GameManager.transform;
            CharSkin = GameManager.Singleton_GameManager.CharacterSkinManager.GetRandomSkin();
            InitSkin();
        }
    }


    private void InitSkin()
    {
        face.sprite = CharSkin.Face;
        for (int i = 0; i < Arm.Length; i++)
        {
            Arm[i].sprite = CharSkin.Arms;
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


    // #region ===================== MENU FUNCTIONS ====================

    public void GoUp(InputAction.CallbackContext _context)
    {
        if (_context.started && _context.interaction is PressInteraction)
        {
            if (MenuTravel.Active)
            {
                MenuTravel.GoUp();
            }
        }
    }


    public void GoRight(InputAction.CallbackContext _context)
    {
        if (_context.started && _context.interaction is PressInteraction)
        {
            if (MenuTravel.Active)
            {
                MenuTravel.GoRight();
            }
            else
            {
                SkinSelector.ChangeSkinRight();
            }
        }
    }


    public void GoDown(InputAction.CallbackContext _context)
    {
        if (_context.started && _context.interaction is PressInteraction)
        {
            if (MenuTravel.Active)
            {
                MenuTravel.GoRight();
            }
        }
    }


    public void GoLeft(InputAction.CallbackContext _context)
    {
        if (_context.started && _context.interaction is PressInteraction)
        {
            if (MenuTravel.Active)
            {
                MenuTravel.GoLeft();
            }
            else
            {
                SkinSelector.ChangeSkinLeft();
            }
        }
    }

    public void Validate(InputAction.CallbackContext _context)
    {
        if (_context.started && _context.interaction is PressInteraction)
        {
            if (MenuTravel.Active)
            {
                MenuTravel.Validate();
            }
            else
            {
                SkinSelector.ValidateSkin();
            }
        }
    }

    // #endregion
}
