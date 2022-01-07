using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EyesController : MonoBehaviour
{
    private List<Player> playersAlive;
    private Vector3 playerPos;
    private int ran;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        playersAlive = PlayersManager.Instance.PlayersAlive;
        ran = Random.Range(0, playersAlive.Count);

    }
    private void Update()
    {
        playersAlive = PlayersManager.Instance.PlayersAlive;
        if (playersAlive != null && playersAlive.Count != 0)
        {
            if (playersAlive[ran]!=null && playersAlive.Count!=0)
            {
                if (playersAlive[ran].PlayerGameState is PlayerGameState.Alive)
                {
                    playerPos = playersAlive[ran].transform.position;

                    Vector3 v = playerPos - transform.position;
                    v.z = 0;
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.down, v);
                    transform.rotation = rotation;
                }
                else
                {
                    if (playersAlive.Count>=0)
                    {
                        ran = Random.Range(0, playersAlive.Count);
                    }
                }
            }
        }
    }
}
