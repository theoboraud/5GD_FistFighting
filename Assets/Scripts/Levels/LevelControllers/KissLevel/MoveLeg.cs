using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveLeg : MonoBehaviour
{
    public bool IsLeftLeg;

    private bool isActiveForce = false;
    private bool isLegMove = false;

    private float rotationZ;
    private float coefRotate;
    private void Start()
    {
        rotationZ = transform.rotation.eulerAngles.z;
        if (IsLeftLeg) coefRotate = 1; else coefRotate = -1;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isActiveForce)
        {
            collision.rigidbody.AddForce((collision.transform.position - transform.position) * 10, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLegMove)
        {
            transform.DORotate(new Vector3(0, 0, rotationZ + coefRotate * 20), 1f).OnComplete(LegKick);
            isLegMove = true;
        }
        if (isActiveForce)
        {
            collision.rigidbody.AddForce((collision.transform.position - transform.position) * 10, ForceMode2D.Impulse);
        }
    }
    private void LegKick()
    {
        isActiveForce = true;
        transform.DORotate(new Vector3(0, 0, rotationZ - coefRotate * 20), 1f).OnComplete(ReturnLeg);
    }
    private void ReturnLeg()
    {
        transform.DORotate(new Vector3(0, 0, rotationZ), 1f).OnComplete(()=>isLegMove = false);
        isActiveForce = false;
    }
}
