using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] MenuTravel menu;

    private void Awake()
    {
        int j = 0;
        int y = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = 0; i < menu.Buttons.Length; i++)
        {
            if(j >= y)
            {
                menu.Buttons[i].gameObject.SetActive(false);
            }
            j++;
        }
    }

    public void LoadScene(int i)
    {
        Reference.multipleCharacterManager.ChangeScene(i);
        Debug.Log("Scene Loaded");
    }
}
