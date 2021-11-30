using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveLeg : MonoBehaviour
{
    public bool IsLeftLeg;
    public bool DisactiveCollisionLegMove;
    public float KickForce;

    private bool isActiveForce = false;
    private bool isLegMove = false;

    private float rotationZ;
    private float coefRotate;
    private void Start()
    {
        rotationZ = transform.rotation.eulerAngles.z;
        if (IsLeftLeg) coefRotate = 1; else coefRotate = -1;
        if (KickForce == 0) KickForce = 3f;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isActiveForce)
        {
            collision.rigidbody.AddForce((collision.transform.position - transform.position) * KickForce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!DisactiveCollisionLegMove)
        {
            LegMove();
        }
        if (isActiveForce)
        {
            collision.rigidbody.AddForce((collision.transform.position - transform.position)* KickForce, ForceMode2D.Impulse);
        }
    }
    private void LegMove()
    {
        if (!isLegMove)
        {
            transform.DORotate(new Vector3(0, 0, rotationZ + coefRotate * 20), 1f).OnComplete(LegKick);
            isLegMove = true;
        }
    }
    private void LegKick()
    {
        isActiveForce = true;
        transform.DORotate(new Vector3(0, 0, rotationZ - coefRotate * 20), 0.3f).OnComplete(StopKick);
    }
    private void StopKick() 
    {
        isActiveForce = false;
        Invoke("ReturnLeg",1f);
    }

    private void ReturnLeg()
    {
        transform.DORotate(new Vector3(0, 0, rotationZ), 1f).OnComplete(()=>isLegMove = false);
    }
    public void ActiveLegMove()
    {
        LegMove();
    }
}
