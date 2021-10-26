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
    [SerializeField] private MultipleCharacterManager mcc;
    public List<int> Skins = new List<int>();
    [SerializeField] GameObject SkinSelector;
    private List<SkinSelector> Selectors = new List<SkinSelector>();

    public int chars = 0;
    private bool ready;

    private void Start()
    {
        mcc = Reference.multipleCharacterManager;
    }

    private void Update()
    {
        if(mcc.characters.Count > chars)
        {
            mcc.characters[chars].RB.simulated = false;
            SpawnSkinSelector(chars);
            chars++;
            Skins.Add(0);
        }
        if(CheckIfAllReady() && ready == false)
        {
            ready = true;
            OpenMenu();
        }
    }

    public CharacterManager LinkCharacterToSelector(int index)
    {
        return mcc.characters[index];
    }

    private void SpawnSkinSelector(int index)
    {
        SkinSelector ss = GameObject.Instantiate(SkinSelector, mcc.characters[index].transform.position, Quaternion.identity, SelectScreen.transform).GetComponent<SkinSelector>();
        ss.transform.parent = SelectScreen.transform;
        Selectors.Add(ss);
    }

    private void RemoveSkinSelector(int index)
    {
        Destroy(Selectors[index].gameObject);
        Selectors.RemoveAt(index);
    }

    public void DestroyAllSkinSelectors()
    {
        for (int i = 0; i < Selectors.Count; i++)
        {
            Destroy(Selectors[i].gameObject);
        }
        Selectors.Clear();
    }

    private bool CheckIfAllReady()
    {
        bool ready = false;
        int a = 0;
        for (int i = 0; i < chars; i++)
        {
            if (Selectors[i].Validated) a++;
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
        foreach (var item in Selectors)
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
        Reference.multipleCharacterManager.QuitGame();
    }
}
