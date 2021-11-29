using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Enums;

public class Menu : MonoBehaviour
{
    //[Header("Stats")]
    //[SerializeField] private bool Menu2D = false;

    [Header("Refs")]
    public List<Button> Buttons = new List<Button>();
    protected int ActiveButtonIndex;
    protected Button ActiveButton;

    public bool Active;

    public NavType NavType;


    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        /*
        int j = 0;
        int y = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (j >= y)
            {
                Buttons[i].gameObject.SetActive(false);
            }
            j++;
        }

        SelectButton(0);*/
    }

    public virtual void GoUp()
    {
        if (NavType == NavType.Vertical)
        {

        }
    }

    public virtual void GoRight()
    {
        if (NavType == NavType.Horizontal)
        {

        }
    }

    public virtual void GoDown()
    {
        if (NavType == NavType.Vertical)
        {

        }
    }

    public virtual void GoLeft()
    {
        if (NavType == NavType.Horizontal)
        {

        }
    }

    public virtual void Validate()
    {
        ActiveButton.onClick.Invoke();
    }

    public virtual void SelectButton(int _buttonIndex)
    {
        UnselectButton();
        ActiveButtonIndex = _buttonIndex;
        ActiveButton = Buttons[_buttonIndex];
        ActiveButton.Select();
        ActiveButton.OnSelect(null);
    }

    public virtual void UnselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        ActiveButton = null;
        ActiveButtonIndex = 0;
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
