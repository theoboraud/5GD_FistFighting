using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static MenuManager Instance;
    [SerializeField] GameObject ScoreScreen;

    [Header("Menu Screens")]
    [SerializeField] CharacterSelectMenu CharacterSelectMenu;
    [SerializeField] MainMenu MainMenu;
    [SerializeField] OptionsMenu OptionsMenu;
    [SerializeField] LevelSelectMenu LevelSelectMenu;

    [Header("Variables")]
    public List<int> CharacterSkinIndex = new List<int>();
    private List<bool> PlayersReady = new List<bool>();
    private int spawnedPlayerCount = 0;
    private Menu ActiveMenu;


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
        switch (GameManager.Instance.GlobalGameState)
        {
            case GlobalGameState.MainMenu:
                GoTo_MainMenu();
                break;
            case GlobalGameState.CharacterSelectMenu:
                GoTo_CharacterSelectMenu();
                break;
            case GlobalGameState.LevelSelectMenu:
                GoTo_LevelSelectMenu();
                break;
            case GlobalGameState.OptionsMenu:
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

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.MainMenu;
    }

    public void GoTo_CharacterSelectMenu()
    {
        ActiveMenu.Deactivate();
        ActiveMenu = CharacterSelectMenu;
        ActiveMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.CharacterSelectMenu;
    }

    public void GoTo_OptionsMenu()
    {
        ActiveMenu.Deactivate();
        ActiveMenu = OptionsMenu;
        ActiveMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.OptionsMenu;
    }

    public void GoTo_LevelSelectMenu()
    {
        ActiveMenu.Deactivate();
        ActiveMenu = LevelSelectMenu;
        ActiveMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.LevelSelectMenu;
    }

    private void DeactivateAllMenu()
    {
        MainMenu.Deactivate();
        CharacterSelectMenu.Deactivate();
        LevelSelectMenu.Deactivate();
        OptionsMenu.Deactivate();
    }

    public void PrintScoreScreen(bool _bool)
    {
        ScoreScreen.SetActive(_bool);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
