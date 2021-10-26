using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTravel : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private bool Menu2D = false;

    [Header("Refs")]
    public Button[] Buttons;
    private Vector2 pos;
    [SerializeField] private Button ActiveButton;

    public bool Active;

    private void OnEnable()
    {
        ButtonActive(pos);
        Invoke("LetsPlay", 0.2f);
    }

    public void GoUp()
    {
        pos -= Vector2.up;
        if (Menu2D)
        {
            pos.y %= Buttons.Length / 2;
            pos.y = Mathf.Abs(pos.y);
        }
        else
        {
            pos.y %= Buttons.Length;
            pos.y = Mathf.Abs(pos.y);
        }
        ButtonActive(pos);
    }

    public void GoDown()
    {
        pos -= Vector2.down;
        if (Menu2D)
        {
            pos.y %= Buttons.Length/2;
            pos.y = Mathf.Abs(pos.y);
        }
        else
        {
            pos.y %= Buttons.Length;
            pos.y = Mathf.Abs(pos.y);
        }
        ButtonActive(pos);
    }

    public void GoLeft()
    {
        if (Menu2D)
        {
            pos += Vector2.left;
            pos.x %= 2;
            pos.x = Mathf.Abs(pos.x);
            ButtonActive(pos);
        }
    }

    public void GoRight()
    {
        if (Menu2D)
        {
            pos += Vector2.right;
            pos.x %= 2;
            pos.x = Mathf.Abs(pos.x);
            ButtonActive(pos);
        }
    }

    public void Validate()
    {
        ActiveButton.onClick.Invoke();
        Debug.Log(ActiveButton.name);
        ActiveButton.image.color = ActiveButton.colors.pressedColor;
    }

    private void ButtonActive(Vector2 pos)
    {
        DeactivateButtons();
        if(Menu2D)
        {
            int i = (int)pos.y + (int)pos.x;
            Buttons[i].image.color = Buttons[i].colors.highlightedColor;
            ActiveButton = Buttons[i];
        }
        else
        {
            Buttons[(int)pos.y].image.color = Buttons[(int)pos.y].colors.highlightedColor;
            ActiveButton = Buttons[(int)pos.y];
        }
    }

    private void DeactivateButtons()
    {
        foreach (var item in Buttons)
        {
            item.image.color = item.colors.normalColor;
        }
    }

    private void LetsPlay()
    {
        Active = true;
    }
}
