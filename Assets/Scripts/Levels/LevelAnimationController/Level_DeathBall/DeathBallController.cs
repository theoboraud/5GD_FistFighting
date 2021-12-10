using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DeathBallController : MonoBehaviour
{
    public float MoveSpeed;

    private Transform deathBallDestinations;
    private List<Transform> destinationsList;

    private int indexDestination;
    private void Start()
    {
        init();
    }
    private void init()
    {
        destinationsList = new List<Transform>();

        deathBallDestinations = transform.Find("DeathBallDestinations");
        for (int i = 0; i < deathBallDestinations.childCount; i++)
        {
            Transform _ballDestination = deathBallDestinations.GetChild(i);
            destinationsList.Add(_ballDestination);
            _ballDestination.GetComponent<SpriteRenderer>().enabled = false;
        }
        indexDestination = 0;

        BallMove();
    }
    private void Update()
    {
        if (transform.position == destinationsList[indexDestination].position)
        {
            ChangeDestination();
        }
    }
    private void BallMove()
    {
        transform.DOMove(destinationsList[indexDestination].position, MoveSpeed).SetSpeedBased().OnComplete(ChangeDestination);
    }

    private void ChangeDestination()
    {
        if (indexDestination < destinationsList.Count)
        {
            indexDestination += 1;
        }
        else
        {
            indexDestination = 0;
        }
    }    
}
