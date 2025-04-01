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
    public EventCenter<PlayerEvent> EventCenter { get; private set; }
    public PlayerData PlayerData { get ; private set ; }
    public PlayerStates PlayerStates { get ; private set; }

    #endregion

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    private PlayerController _playerController;
    private PlayerFeedbackManager _playerFeedbackManager;
    private PlayerUI _playerUI;
    [SerializeField] private CharacterSkin Skin;
    private int skinIndex;

    [SerializeField] private SpriteRenderer Face_SpriteRenderer;
    [SerializeField] private SpriteRenderer[] Arms_SpriteRenderers;

    public SpriteRenderer Outline_SpriteRenderer;
    public PlayerVoiceController VoiceController;

    public bool isReady = false;

    // Contains the index of the current skin

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

        _playerFeedbackManager = GetComponent<PlayerFeedbackManager>();
        _playerFeedbackManager.Init();

        _playerUI = GetComponentInChildren<PlayerUI>();
        Color playerColor = GlobalSettings.PlayerColors[_playerIndex];

        Outline_SpriteRenderer.color = playerColor;
        _playerUI.AddPlayerUI(_playerIndex, playerColor);

        ///This we have to do in PlayerManager, TODO
        if (PlayersManager.Instance.Players.Count < 4)
        {
            // Keep the player game object between scenes
            DontDestroyOnLoad(gameObject);

            // Get a random skin at start -> TODO: Select skin
            skinIndex = Random.Range(0, PlayersManager.Instance.SkinsData.CharacterSkins.Count - 1);
            ChangeSkin(PlayersManager.Instance.SkinsData.GetSkin(skinIndex));
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Set the layer of the player
        PlayerData.PlayerIndex = _playerIndex;
        gameObject.layer = LayerMask.NameToLayer($"Player{_playerIndex+1}");
    }


    /// <summary>
    ///
    /// </summary>
    private void InitSkin()
    {
        Face_SpriteRenderer.sprite = Skin.SpriteFace;
        Outline_SpriteRenderer.sprite = Skin.SpriteFace;

        for (int i = 0; i < Arms_SpriteRenderers.Length; i++)
        {
            Arms_SpriteRenderers[i].sprite = Skin.SpriteArm;
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
        Skin = _charSkin;
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
        Skin = PlayersManager.Instance.SkinsData.GetSkin(skinIndex);

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
        Skin = PlayersManager.Instance.SkinsData.GetSkin(skinIndex);

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
        PlayerStates.PlayerGameState = PlayerGameState.Alive;
        gameObject.layer = LayerMask.NameToLayer($"Player{PlayerData.PlayerIndex + 1}");
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

        PlayerStates.PlayerGameState = PlayerGameState.Dead;

        _playerUI.UpdateLivesUI();

        //Broadcast global of player dead
        GEventCenter.Invoke<Player>(GameEvent.OnPlayerDead, this);

        //tmp, to change //TODO
        OnGameRoundStart();
    }


    /// <summary>
    ///     Stun the player
    /// </summary>
    public void Hit()
    {
        PlayerStates.PlayerPhysicState = PlayerPhysicState.IsHit;
    }

    private void OnGameRoundStart()
    {
        isReady = false;
        _playerUI.GetReady(false);
    }

    /// <summary>
    ///     Indicate if the player is ready in the lobby
    /// </summary>
    public void OnReadyInput()
    {
        isReady = !isReady;

        _playerUI.GetReady(isReady);

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
        return Skin;
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
