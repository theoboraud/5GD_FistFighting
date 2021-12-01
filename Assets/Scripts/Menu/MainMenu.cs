using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums;

public class MainMenu : MonoBehaviour
{
    public List<Button> Buttons = new List<Button>();
    private Button activeButton;
    private int activeButtonIndex = 0;


    public void Init()
    {
        activeButtonIndex = 0;
        SelectActiveButton();
    }


    private void SelectActiveButton()
    {
        activeButton = Buttons[activeButtonIndex];

        EventSystem.current.SetSelectedGameObject(activeButton.gameObject);
    }


    public virtual void UnselectActiveButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        activeButton = null;
    }


    public void GoUp()
    {
        UnselectActiveButton();

        activeButtonIndex -= 1;
        if (activeButtonIndex < 0)
        {
            activeButtonIndex = Buttons.Count - 1;
        }

        SelectActiveButton();
    }


    public void GoDown()
    {
        UnselectActiveButton();

        activeButtonIndex += 1;
        if (activeButtonIndex >= Buttons.Count)
        {
            activeButtonIndex = 0;
        }

        SelectActiveButton();
    }


    public void Validate()
    {
        activeButton.onClick.Invoke();
    }


    public void ButtonPlay()
    {
        Deactivate();
        GameManager.Instance.Play();
        LevelManager.Instance.LoadLobbyLevel();
    }


    public void ButtonOptions()
    {
        // TODO
    }


    public void ButtonQuit()
    {
        GameManager.Instance.QuitGame();
    }


    public void Activate()
    {
        Init();
        this.gameObject.SetActive(true);
    }


    public void Deactivate()
    {
        UnselectActiveButton();
        this.gameObject.SetActive(false);
    }
}
