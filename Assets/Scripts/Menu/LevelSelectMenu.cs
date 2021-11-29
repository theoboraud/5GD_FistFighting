using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class LevelSelectMenu : Menu
{
    public override void Init()
    {
        base.Init();
        this.NavType = NavType.TwoDimensions;
    }

    public void ButtonPlay()
    {
        //MenuManager.Instance.GoTo_CharacterSelectMenu();
    }

    public void ButtonOptions()
    {
        // TODO
    }

    public void ButtonQuit()
    {
        GameManager.Instance.QuitGame();
    }
}
