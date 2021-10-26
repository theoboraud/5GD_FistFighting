using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    [SerializeField] List<Transform> SpawnPoints = new List<Transform>();

    private void Start()
    {
        if(GameManager.Singleton_GameManager != null)
        {
            GameManager.Singleton_GameManager.SpawnPoints = this.SpawnPoints;
            GameManager.Singleton_GameManager.PlacePlayers();
        }

    }
}
