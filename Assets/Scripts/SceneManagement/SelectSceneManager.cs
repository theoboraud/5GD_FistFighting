using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    //Loads Selected Level
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public int GetScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
