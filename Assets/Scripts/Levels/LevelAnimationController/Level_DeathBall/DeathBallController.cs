using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DeathBallController : MonoBehaviour
{
    [Tooltip("Death ball moving speed, from 0-1, if >=1, the ball will teleport to next position directly")] 
    public float MoveSpeed;

    private Transform deathBall;
    private Transform deathBallDestinations;
    private List<Transform> destinationsList;

    private Vector3 startPosition;
    private int indexDestination;
    private int lockDestinationIndex = 4; //Index for destination lock count, init for 4 (4 destination in use)
    private float lerpScale = 0;

    private bool canMove;
    private void Start()
    {
        init();
    }
    private void init()
    {
        destinationsList = new List<Transform>();

        deathBall = transform.Find("DeathBall");
        deathBallDestinations = transform.Find("DeathBallDestinations");
        for (int i = 0; i < deathBallDestinations.childCount; i++)
        {
            Transform _ballDestination = deathBallDestinations.GetChild(i);
            destinationsList.Add(_ballDestination);
            _ballDestination.GetComponent<SpriteRenderer>().enabled = false;
        }
        indexDestination = 0;
        startPosition = deathBall.position;

        canMove = true;
    }
    private void BallMove()
    {
        deathBall.DOMove(destinationsList[indexDestination].position, MoveSpeed).SetEase(Ease.Linear).SetSpeedBased().OnComplete(ChangeDestination);
    }

    private void Update()
    {
        if (canMove)
        {
            deathBall.position = Vector3.Lerp(startPosition, destinationsList[indexDestination].position,lerpScale);
            lerpScale += MoveSpeed;

            if (lerpScale>=1)
            {
                lerpScale = 0;
                startPosition = destinationsList[indexDestination].position;
                ChangeDestination();
            }
        }
    }

    private void ChangeDestination()
    {
        if (indexDestination < lockDestinationIndex-1)
        {
            indexDestination += 1;
        }
        else
        {
            indexDestination = 0;
        }
    }
    
    public void UnlockDestinations()
    {
        lockDestinationIndex = destinationsList.Count;
    }
}
