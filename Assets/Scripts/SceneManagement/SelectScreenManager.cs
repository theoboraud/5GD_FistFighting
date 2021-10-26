using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScreenManager : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] GameObject SelectScreen;
    [SerializeField] GameObject MenuScreen;
    [SerializeField] GameObject OptionScreen;
    [SerializeField] GameObject LevelScreen;
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    public List<int> Skins = new List<int>();
    [SerializeField] GameObject SkinSelector;
    private List<SkinSelector> SkinSelectors = new List<SkinSelector>();

    public int chars = 0;
    private bool ready;

    private void Awake()
    {
        gameManager = GameManager.Singleton_GameManager;
    }

    private void Update()
    {
        if (gameManager.Players.Count > chars)
        {
            gameManager.Players[chars].RB.simulated = false;
            SpawnSkinSelector(chars);
            chars++;
            Skins.Add(0);
        }
        if (CheckIfAllReady() && ready == false)
        {
            ready = true;
            OpenMenu();
        }
    }

    public Player LinkCharacterToSelector(int _index)
    {
        return gameManager.Players[_index];
    }

    private void SpawnSkinSelector(int _index)
    {
        SkinSelector _skinSelector = GameObject.Instantiate(SkinSelector, gameManager.Players[_index].transform.position, Quaternion.identity, SelectScreen.transform).GetComponent<SkinSelector>();
        _skinSelector.transform.parent = SelectScreen.transform;
        _skinSelector.Player = GameManager.Singleton_GameManager.Players[_index];
        GameManager.Singleton_GameManager.Players[_index].SkinSelector = _skinSelector;
        SkinSelectors.Add(_skinSelector);
    }

    private void RemoveSkinSelector(int _index)
    {
        Destroy(SkinSelectors[_index].gameObject);
        SkinSelectors.RemoveAt(_index);
    }

    public void DestroyAllSkinSkinSelectors()
    {
        for (int i = 0; i < SkinSelectors.Count; i++)
        {
            Destroy(SkinSelectors[i].gameObject);
        }
        SkinSelectors.Clear();
    }

    private bool CheckIfAllReady()
    {
        bool ready = false;
        int a = 0;
        for (int i = 0; i < chars; i++)
        {
            if (SkinSelectors[i].Validated) a++;
        }
        if (a == chars && a > 0) ready = true;
        return ready;
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
        foreach (var item in SkinSelectors)
        {
            item.Validated = false;
        }
        MenuScreen.SetActive(false);
        SelectScreen.SetActive(true);
        LevelScreen.SetActive(false);
        OptionScreen.SetActive(false);
    }

    public void Quit()
    {
        gameManager.QuitGame();
    }
}
