using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IntroAnimScene : MonoBehaviour
{
    public bool stopAnim = false;
    public FMODUnity.StudioEventEmitter introSound;
    private InputAction leftMouseClick;

    private void Awake()
    {
        leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => EndIntro();
        leftMouseClick.Enable();
    }

    private void EndIntro()
    {
        SceneManager.LoadScene(3);
        introSound.Stop();
        stopAnim = false;
    }
}
