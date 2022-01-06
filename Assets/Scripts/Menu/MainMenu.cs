using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums;

public class MainMenu : MonoBehaviour
{
    public List<Button> Buttons = new List<Button>();
    public GameObject Selector;
    private Button activeButton;
    private int activeButtonIndex = 0;
    [SerializeField] private float selectedIncrementation = 50;


    public void Init()
    {
        UnselectActiveButton();
        activeButtonIndex = 0;
        SelectActiveButton();
        //Selector.transform.position = new Vector3(Buttons[0].transform.position.x - selectedIncrementation, Buttons[0].transform.position.y, Buttons[0].transform.position.z);
    }


    private void SelectActiveButton()
    {
        activeButton = Buttons[activeButtonIndex];
        Selector.transform.position = activeButton.transform.position;
        activeButton.transform.position = new Vector3(activeButton.transform.position.x + selectedIncrementation, activeButton.transform.position.y, activeButton.transform.position.z);
        EventSystem.current.SetSelectedGameObject(activeButton.gameObject);
    }


    public virtual void UnselectActiveButton()
    {
        if (activeButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            activeButton.transform.position = new Vector3(activeButton.transform.position.x - selectedIncrementation, activeButton.transform.position.y, activeButton.transform.position.z);
            activeButton = null;
        }
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
        GameManager.Instance.PlayMode();
        LevelManager.Instance.LoadLobbyLevel();
    }


    public void ButtonHistory()
    {
        // TODO
    }


    public void ButtonOptions()
    {
        // TODO
    }


    public void ButtonCredits()
    {
        // TODO
    }


    public void ButtonQuit()
    {
        GameManager.Instance.QuitGame();
    }


    public void Activate()
    {
        this.gameObject.SetActive(true);
        GameManager.Instance.MenuMode(GlobalGameState.MainMenu);
        Invoke("Init", 0.5f);
    }


    public void Deactivate()
    {
        UnselectActiveButton();
        this.gameObject.SetActive(false);
    }
}
