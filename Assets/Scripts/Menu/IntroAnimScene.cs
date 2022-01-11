using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimScene : MonoBehaviour
{
    public bool stopAnim = false;
    public FMODUnity.StudioEventEmitter introSound;

    private void Update()
    {
        if (stopAnim)
        {
            SceneManager.LoadScene(3);
            introSound.Stop();
            stopAnim = false;
        }
    }
}
