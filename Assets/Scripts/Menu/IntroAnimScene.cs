using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimScene : MonoBehaviour
{
    private void Awake()
    {
        Invoke("EndAnimIntro", 21) ;
    }

    public void EndAnimIntro()
    {
        SceneManager.LoadScene(2);
    }
}
