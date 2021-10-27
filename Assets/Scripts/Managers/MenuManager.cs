using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static MenuManager Instance;

    [Header("Menu Screens")]
    [SerializeField] CharacterSelectMenu CharacterSelectMenu;
    [SerializeField] MainMenu MainMenu;
    [SerializeField] OptionsMenu OptionsMenu;
    [SerializeField] LevelSelectMenu LevelSelectMenu;
    private Menu ActiveMenu;

    [Header("Variables")]
    public List<int> CharacterSkinIndex = new List<int>();
    private List<bool> PlayersReady = new List<bool>();
    private int spawnedPlayerCount = 0;


    // Init singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitMenu();
    }

    private void InitMenu()
    {
        DeactivateAllMenu();
        switch (GameManager.Instance.GameState)
        {
            case GameState.MainMenu:
                GoTo_MainMenu();
                break;
            case GameState.CharacterSelectMenu:
                GoTo_CharacterSelectMenu();
                break;
            case GameState.LevelSelectMenu:
                GoTo_LevelSelectMenu();
                break;
            case GameState.OptionsMenu:
                GoTo_OptionsMenu();
                break;
            default:
                break;
        }
    }

    public void GoTo_MainMenu()
    {
        if (ActiveMenu != null)
        {
            ActiveMenu.Deactivate();
        }
        ActiveMenu = MainMenu;
        ActiveMenu.Activate();

        // Update GameState
        GameManager.Instance.GameState = GameState.MainMenu;
    }

    public void GoTo_CharacterSelectMenu()
    {
        ActiveMenu.Deactivate();
        ActiveMenu = CharacterSelectMenu;
        ActiveMenu.Activate();

        // Update GameState
        GameManager.Instance.GameState = GameState.CharacterSelectMenu;
    }

    public void GoTo_OptionsMenu()
    {
        ActiveMenu.Deactivate();
        ActiveMenu = OptionsMenu;
        ActiveMenu.Activate();

        // Update GameState
        GameManager.Instance.GameState = GameState.OptionsMenu;
    }

    public void GoTo_LevelSelectMenu()
    {
        ActiveMenu.Deactivate();
        ActiveMenu = LevelSelectMenu;
        ActiveMenu.Activate();

        // Update GameState
        GameManager.Instance.GameState = GameState.LevelSelectMenu;
    }

    private void DeactivateAllMenu()
    {
        MainMenu.Deactivate();
        CharacterSelectMenu.Deactivate();
        LevelSelectMenu.Deactivate();
        OptionsMenu.Deactivate();
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
