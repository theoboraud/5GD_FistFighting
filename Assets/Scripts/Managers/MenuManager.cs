using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private float startingTimer = 0f;

    [Header("Starting Timer")]
    public GameObject UI_StartingTimer;
    public GameObject Text_StartingTimer;


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

    private void Update()
    {
        if (startingTimer > 0f)
        {
            // Clamp the timer to minimum 0 and maximum 3
            startingTimer = Mathf.Clamp(startingTimer - Time.deltaTime, 0f, 3f);

            // If timer reached 0, spawn the players
            if (startingTimer == 0f)
            {
                PlayersManager.Instance.SpawnAllPlayers();
                UI_StartingTimer.SetActive(false);
            }
            // If the timer is less or equal to 1, the timer shows 1
            else if (startingTimer <= 1f)
            {
                Text_StartingTimer.GetComponent<Text>().text = "1";
            }
            // If the timer is less or equal to 2, the timer shows 2
            else if (startingTimer <= 2f)
            {
                Text_StartingTimer.GetComponent<Text>().text = "2";
            }
            // If the timer is less or equal to 3, the timer shows 3
            else if (startingTimer <= 3f)
            {
                Text_StartingTimer.GetComponent<Text>().text = "3";
            }
        }
    }

    public void StartTimer()
    {
        UI_StartingTimer.SetActive(true);
        startingTimer = 3f;
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
