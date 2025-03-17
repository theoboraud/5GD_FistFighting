using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private void OnEnable()
    {
        List<Transform> SpawnPoints = new List<Transform>();

        foreach (Transform _child in transform)
        {
            SpawnPoints.Add(_child);
        }

        EventCenter.Invoke<List<Transform>>(GameEvent.OnSpawnPointsInit, SpawnPoints);
    }
}
