using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private void Awake()
    {
        if(LevelManager.Instance != null)
        {
            LevelManager.Instance.InitSpawnPoints();
        }
    }
}
