using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    [SerializeField] List<Transform> SpawnPoints = new List<Transform>();

    private void Start()
    {
        if(Reference.multipleCharacterManager != null)
            Reference.multipleCharacterManager.SpawnPoints = this.SpawnPoints;
    }
}
