using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KickLeg : MonoBehaviour
{
    private Vector3 location;
    private Vector3 movePos;
    private float moveDis;
    private void Start()
    {
        moveDis = 3f;
        location = transform.position;
        movePos = new Vector3(moveDis, moveDis, 0);
    }
    public void StartKick()
    {

    }
}
