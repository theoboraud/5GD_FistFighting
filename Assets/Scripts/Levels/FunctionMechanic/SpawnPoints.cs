using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
	List<Transform> _spawnPointList = new List<Transform>();
	
    private void OnEnable()
    {
        foreach (Transform _child in transform)
        {
	        _spawnPointList.Add(_child);
        }
    }

    private void Start()
    {
	    GEventCenter.Invoke<List<Transform>>(GameEvent.OnSpawnPointsInit, _spawnPointList);
    }
}
