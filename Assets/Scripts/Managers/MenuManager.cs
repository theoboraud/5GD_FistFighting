using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static MenuManager Instance;

    [Header("Menu Screens")]
    [SerializeField] GameObject SelectScreen;
    [SerializeField] GameObject MenuScreen;
    [SerializeField] GameObject OptionScreen;
    [SerializeField] GameObject LevelScreen;

    [Header("Variables")]
    public List<int> CharacterSkinIndex = new List<int>();
    private List<bool> PlayersReady = new List<bool>();
    private int spawnedPlayerCount = 0;


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
    }


    private void Update()
    {
        if (PlayersManager.Instance.Players.Count > spawnedPlayerCount)
        {
            PlayerManager.Instance.Players[spawnedPlayerCount].RB.simulated = false;
            SpawnSkinSelector(spawnedPlayerCount);
            spawnedPlayerCount++;
            CharacterSkinIndex.Add(Random.Range(0, PlayersManager.Instance.SkinsData.CharacterSkins.Count));
        }
        if (GameManager.Instance.GameState is SelectMenu && PlayersManager.Instance.AllPlayersReady())
        {
            OpenMenu();
        }
    }


    public void OpenMenu()
    {
        SelectScreen.SetActive(false);
        LevelScreen.SetActive(false);
        OptionScreen.SetActive(false);
        MenuScreen.SetActive(true);
    }

    public void OpenLevelSelect()
    {
        MenuScreen.SetActive(false);
        SelectScreen.SetActive(false);
        OptionScreen.SetActive(false);
        LevelScreen.SetActive(true);
    }

    public void OpenOptions()
    {
        MenuScreen.SetActive(false);
        SelectScreen.SetActive(false);
        LevelScreen.SetActive(false);
        OptionScreen.SetActive(true);
    }

    public void OpenSelectScreen()
    {
        /*foreach (var item in SkinSelectors)
        {
            item.Validated = false;
        }*/
        MenuScreen.SetActive(false);
        SelectScreen.SetActive(true);
        LevelScreen.SetActive(false);
        OptionScreen.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
