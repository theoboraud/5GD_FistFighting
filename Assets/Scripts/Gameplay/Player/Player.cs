using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using UnityEngine.UI;
using Enums;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using System.Reflection;

/// <summary>
/// Main class of each player, manager all player's data, game state,and behaviours
/// </summary>
public class Player : MonoBehaviour
{
    #region EVENTS

    //public event CallBack onInit;
    //public event CallBack onSpawn;
    //public event CallBack onHit;
    //public event CallBack onKilled;
    //public event CallBack onInvincibilityStart;
    //public event CallBack onInvincibilityStop;
    public EventCenter<PlayerEvent> EventCenter { get; private set; }
    public PlayerData PlayerData { get ; private set ; }
    public PlayerStates PlayerStates { get ; private set; }

    #endregion

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    private PlayerController _playerController;
    private PlayerFeedbackManager _playerFeedbackManager;
    [SerializeField] private CharacterSkin _skin;
    private int skinIndex;

    [SerializeField] private SpriteRenderer Face_SpriteRenderer;
    [SerializeField] private SpriteRenderer[] Arms_SpriteRenderers;


    public SpriteRenderer Outline_SpriteRenderer;
    public GameObject PlayerIndicator;
    public GameObject GO_IsReady;
    public PlayerVoiceController VoiceController;
    public FeedbackFaceController FaceController;

    public bool isReady = false;

    // Contains the index of the current skin

    [System.NonSerialized] public string PlayerLayer;

    // #endregion


    // #region ==================== INIT FUNCTIONS ====================

    /// <summary>
    /// Init by Player Manager at Game round Start
    /// </summary>
    public void StartInit(int _playerIndex)
    {
        EventCenter = new EventCenter<PlayerEvent>();

        PlayerData = new PlayerData(this);
        PlayerStates = new PlayerStates(this);

        _playerController = GetComponent<PlayerController>();
        _playerController.Init();

        ///This we have to do in PlayerManager, TODO
        if (PlayersManager.Instance.Players.Count < 4)
        {
            // Keep the player game object between scenes
            DontDestroyOnLoad(gameObject);

            // Add player to the PlayersManager
            PlayersManager.Instance.AddPlayer(this);


            // Get a random skin at start -> TODO: Select skin
            skinIndex = Random.Range(0, PlayersManager.Instance.SkinsData.CharacterSkins.Count - 1);
            ChangeSkin(PlayersManager.Instance.SkinsData.GetSkin(skinIndex));
        }
        else
        {
            Destroy(this.gameObject);
        }

        PlayerData.PlayerIndex = _playerIndex;
        gameObject.layer = LayerMask.NameToLayer($"Player{_playerIndex+1}");
    }
    
    /// <summary>
    /// Call on a new level start
    /// </summary>
    public void InitPlayer()
    {
        PlayerData.PlayerLives = GameManager.Instance.ParamData.PARAM_Player_Lives;

    }


    /// <summary>
    ///
    /// </summary>
    private void InitSkin()
    {
        Face_SpriteRenderer.sprite = _skin.SpriteFace;
        Outline_SpriteRenderer.sprite = _skin.SpriteFace;

        for (int i = 0; i < Arms_SpriteRenderers.Length; i++)
        {
            Arms_SpriteRenderers[i].sprite = _skin.SpriteArm;
        }

        //MenuManager.Instance.PlayerScores[PlayersManager.Instance.Players.IndexOf(this)].SetFace(this);
    }


    // #endregion


    // #region ==================== SKIN FUNCTIONS ====================

    /// <summary>
    ///
    /// </summary>
    public void ChangeSkin(CharacterSkin _charSkin)
    {
        _skin = _charSkin;
        InitSkin();
    }


    /// <summary>
    ///     Change character skin to the next one in the SkinData library
    /// </summary>
    public void NextCharacter()
    {
        skinIndex = skinIndex + 1;
        if (skinIndex >= PlayersManager.Instance.SkinsData.CharacterSkins.Count)
        {
            skinIndex = 0;
        }
        _skin = PlayersManager.Instance.SkinsData.GetSkin(skinIndex);

        InitSkin();
    }


    /// <summary>
    ///     Change character skin to the previous one in the SkinData library
    /// </summary>
    public void PreviousCharacter()
    {
        skinIndex = skinIndex - 1;
        if (skinIndex < 0)
        {
            skinIndex = PlayersManager.Instance.SkinsData.CharacterSkins.Count - 1;
        }
        _skin = PlayersManager.Instance.SkinsData.GetSkin(skinIndex);

        InitSkin();
    }

    // #endregion


    // #region ==================== PLAYER FUNCTIONS ====================

    /// <summary>
    ///     Enable the player's sprite renderer and set its position to _targetPos
    /// </summary>
    public void Spawn(Vector3 _targetPos)
    {
        EventCenter.Invoke(PlayerEvent.OnPlayerSpawn);
        Face_SpriteRenderer.enabled = true;
        this.transform.position = _targetPos;

        if (GameManager.Instance.GlobalGameState == GlobalGameState.InPlay)
        {
            //Set player invincible
            PlayerStates.PlayerGameState = PlayerGameState.Invincible;
            gameObject.layer = LayerMask.NameToLayer("Invincible");
            Invoke(nameof(StopInvincibility), GlobalSettings.PlayerInvincibility);
        }
    }

    private void StopInvincibility()
    {
        EventCenter.Invoke(PlayerEvent.OnInvinciblityStop);
    }


    /// <summary>
    ///     Disable the player's sprite renderer and set its position to somewhere far from the map (to change?)
    /// </summary>
    public void Kill()
    {
        if (PlayerStates.PlayerGameState is PlayerGameState.Dead) return;

        Face_SpriteRenderer.enabled = false;
        this.transform.position = new Vector3(1000, 1000, 0);

        PlayerData.PlayerLives -= 1;

        IsReadyUI(false);

        PlayerStates.PlayerGameState = PlayerGameState.Dead;

        //Broadcast global of player dead
        GEventCenter.Invoke<Player>(GameEvent.OnPlayerDead, this);
    }


    /// <summary>
    ///     Stun the player
    /// </summary>
    public void Hit()
    {
        PlayerStates.PlayerPhysicState = PlayerPhysicState.IsHit;
    }

    /// <summary>
    ///     Indicate if the player is ready in the lobby
    /// </summary>
    public void IsReadyUI(bool _bool)
    {
        GO_IsReady.SetActive(_bool);

        isReady = _bool;

        // If all players are ready, end the round
        if (PlayersManager.Instance.AllPlayersReady())
        {
            //GameManager.Instance.EndOfRound(null);
            MenuManager.Instance.ReadyTimer = 3f;
            MenuManager.Instance.UI_ReadyTimer.SetActive(true);
        }
    }

    // #endregion

    public CharacterSkin GetSkin()
    {
        return _skin;
    }

    public PlayerController GetPlayerController()
    {
        return _playerController;
    }

    #region Get States

    public bool IsAlive()
    {
        return PlayerStates.PlayerGameState == PlayerGameState.Alive;
    }

    public bool IsDead()
    {
        return PlayerStates.PlayerGameState == PlayerGameState.Dead;
    }

    public bool IsInvincible()
    {
        return PlayerStates.PlayerGameState == PlayerGameState.Invincible;
    }

    public bool IsInAir()
    {
        return PlayerStates.PlayerPhysicState == PlayerPhysicState.InAir;
    }

    public bool IsGrounded()
    {
        return PlayerStates.PlayerPhysicState == PlayerPhysicState.OnGround;
    }

    public bool IsHit()
    {
        return PlayerStates.PlayerPhysicState == PlayerPhysicState.IsHit;
    }

    #endregion

}
