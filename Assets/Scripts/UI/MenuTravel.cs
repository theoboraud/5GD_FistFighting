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
    [SerializeField] Button actifButton;

    bool actif;

    private void OnEnable()
    {
        ButtonActive(pos);
        Invoke("LetsPlay", 0.2f);
    }

    public void GoUp(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if(ctx.started && actif)
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
    }

    public void GoDown(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (ctx.started && actif)
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
    }

    public void GoLeft(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (ctx.started && actif)
        {
            if (Menu2D)
            {
                pos += Vector2.left;
                pos.x %= 2;
                pos.x = Mathf.Abs(pos.x);
                ButtonActive(pos);
            }
        } 
    }

    public void GoRight(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (ctx.started && actif)
        {
            if (Menu2D)
            {
                pos += Vector2.right;
                pos.x %= 2;
                pos.x = Mathf.Abs(pos.x);
                ButtonActive(pos);
            }
        }
            
    }

    public void Validate(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if(ctx.started && actif)
        {
            actifButton.onClick.Invoke();
            Debug.Log(actifButton.name);
            actifButton.image.color = actifButton.colors.pressedColor;
        }
    }

    private void ButtonActive(Vector2 pos)
    {
        DeactivateButtons();
        if(Menu2D)
        {
            int i = (int)pos.y + (int)pos.x;
            Buttons[i].image.color = Buttons[i].colors.highlightedColor;
            actifButton = Buttons[i];
        }
        else
        {
            Buttons[(int)pos.y].image.color = Buttons[(int)pos.y].colors.highlightedColor;
            actifButton = Buttons[(int)pos.y];
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
        actif = true;
    }
}
