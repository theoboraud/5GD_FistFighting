using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KickLeg : MonoBehaviour
{
    private Vector3 location;
    private Vector3 movePos;
    private float moveDis;
    private float coeffLeft;
    private bool onKick;

    public bool IsLeftLeg;
    public float KickForce;

    private void Start()
    {
        moveDis = 3f;
        location = transform.position;
        if (IsLeftLeg) coeffLeft = 1; else coeffLeft = -1;
        movePos = new Vector3(moveDis*coeffLeft, moveDis, 0);
        onKick = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.AddForce((collision.transform.position - transform.position) * KickForce, ForceMode2D.Impulse);
    }
    public void StartKick()
    {
        if (!onKick)
        {
            onKick = true;
            transform.DOMove(transform.position + movePos, 0.3f);
            Invoke("ReturnLeg", 1f);
        }
    }
    private void ReturnLeg()
    {
        transform.DOMove(location, 1f).OnComplete(() => onKick = false);
        
    }
    private void Recharge()
    {
        onKick = false;
    }
}
