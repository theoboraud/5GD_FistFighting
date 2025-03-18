using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using UnityEngine.UI;
using Enums;
using UnityEngine.Serialization;

/// <summary>
///     Class used to spawn the player arms during gameplay
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

    #endregion

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    private PlayerController _playerController;
    private PlayerFeedbackManager _playerFeedbackManager;
    private PlayerData _playerData;
    [SerializeField] private CharacterSkin _skin;
    private int skinIndex;  
    
    [SerializeField] private SpriteRenderer Face_SpriteRenderer;
    [SerializeField] private SpriteRenderer[] Arms_SpriteRenderers;
    
    
    public SpriteRenderer Outline_SpriteRenderer;
    public GameObject PlayerIndicator;
    public GameObject GO_IsReady;
    public PlayerVoiceController VoiceController;
    public FeedbackFaceController FaceController;
    
    
    
                           // Contains the index of the current skin
    
    [System.NonSerialized] public string PlayerLayer;

    // #endregion


    // #region ==================== INIT FUNCTIONS ====================

    private void Awake()
    {
        EventCenter = new EventCenter<PlayerEvent>();
    }

    /// <summary>
    ///     Init variables
    /// </summary>
    public void OnEnable()
    {
	    _playerController = GetComponent<PlayerController>();
	    _playerData = GetComponent<PlayerData>();

        EventCenter.Invoke(PlayerEvent.OnPlayerInit);
	    
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

        if (_playerData.nbDeath > 0)
        {
            EventCenter.Invoke(PlayerEvent.OnInvinciblityStart);
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
        if (PlayersManager.Instance.PlayersAlive.Contains(this))
        {
            EventCenter.Invoke(PlayerEvent.OnPlayerKilled);

            Face_SpriteRenderer.enabled = false;
            this.transform.position = new Vector3(1000, 1000, 0);

            
            IsReadyUI(false);
            

            // Remove the player from the PlayersAlive reference in PlayersManager
            PlayersManager.Instance.KillPlayer(this);
        }
    }


    /// <summary>
    ///     Stun the player
    /// </summary>
    public void Hit()
    {
        EventCenter.Invoke(PlayerEvent.OnPlayerHit);
    }
    


    /// <summary>
    ///     Indicate if the player is ready in the lobby
    /// </summary>
    public void IsReadyUI(bool _bool)
    {
        GO_IsReady.SetActive(_bool);

        if (_bool)
        {
            IsReady = true;
        }
        else
        {
            IsReady = false;
        }

        // If all players are ready, end the round
        if (PlayersManager.Instance.AllPlayersReady())
        {
            //GameManager.Instance.EndOfRound(null);
            MenuManager.Instance.ReadyTimer = 3f;
            MenuManager.Instance.UI_ReadyTimer.SetActive(true);
        }
    }

    // #endregion
}
