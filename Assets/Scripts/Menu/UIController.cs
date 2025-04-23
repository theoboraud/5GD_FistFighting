using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages showing and hiding UI screens, ensuring only one is active at a time.
/// </summary>
public class UIController : MonoBehaviour
{
    private List<GameObject> uiScreens = new List<GameObject>();

    /// <summary>
    /// Registers a UI screen to be managed.
    /// </summary>
    public void RegisterScreen(GameObject screen)
    {
        if (!uiScreens.Contains(screen))
        {
            uiScreens.Add(screen);
        }
    }

    /// <summary>
    /// Show one screen and hide all others.
    /// </summary>
    public void ShowOnly(GameObject screenToShow)
    {
        foreach (var screen in uiScreens)
        {
            screen.SetActive(screen == screenToShow);
        }
    }

    /// <summary>
    /// Hide all registered screens.
    /// </summary>
    public void HideAll()
    {
        foreach (var screen in uiScreens)
        {
            screen.SetActive(false);
        }
    }
}
