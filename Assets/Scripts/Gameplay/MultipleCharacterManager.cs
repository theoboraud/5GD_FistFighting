using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCharacterManager : MonoBehaviour
{
    [Header("References")]
    public List<CharacterManager> characters = new List<CharacterManager>();
    public List<Transform> SpawnPoints = new List<Transform>();
    public CharacterSkinManager characterSkinManager;
    public SelectSceneManager sceneManager;

    private int i;

    private void Awake()
    {
        if(Reference.multipleCharacterManager == null)
        {
            Reference.multipleCharacterManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    //Function that's called when Avatar is spawned
    //Allows this script to manage all players
    public void AddCharacter(CharacterManager item)
    {
        characters.Add(item);
        item.transform.position = SpawnPoints[i].position;
        i++;
    }

    //Function that allows the game to change level
    public void ChangeScene(int sceneIndex)
    {
        if (sceneIndex > 0)
        {
            foreach (var item in characters)
            {
                item.RB.simulated = true;
            }
        }
        sceneManager.LoadScene(sceneIndex);
        i = 0;
    }

    public void PlaceCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].transform.position = SpawnPoints[i].position;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
