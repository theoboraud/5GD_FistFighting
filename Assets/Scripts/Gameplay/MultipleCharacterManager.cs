using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleCharacterManager : MonoBehaviour
{
    [Header("References")]
    public List<CharacterManager> characters = new List<CharacterManager>();
    public List<Transform> SpawnPoints = new List<Transform>();
    public CharacterSkinManager characterSkinManager;

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

    public void AddCharacter(CharacterManager item)
    {
        characters.Add(item);
        item.transform.position = SpawnPoints[i].position;
        i++;
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        i = 0;
    }
}
