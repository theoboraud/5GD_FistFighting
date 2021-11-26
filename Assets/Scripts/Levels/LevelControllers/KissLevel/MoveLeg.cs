using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveLeg : MonoBehaviour
{
    public bool IsLeftLeg;

    private bool isActiveForce = false;
    private float rotationZ;
    private float coefRotate;
    private void Start()
    {
        rotationZ = transform.rotation.z;
        if (IsLeftLeg) coefRotate = 1; else coefRotate = -1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        transform.DORotate(new Vector3 (0,0, rotationZ + coefRotate * 20),1f);

        if (isActiveForce)
        {
            collision.rigidbody.AddForce((collision.transform.position - transform.position) * 10, ForceMode2D.Impulse);

        }
        Invoke("LegKick", 1);

    }
    private void LegKick()
    {
        isActiveForce = true;
        transform.DORotate(new Vector3(0, 0, rotationZ - coefRotate * 20), 1f);
        Invoke("ReturnLeg", 1);
    }
    private void ReturnLeg()
    {
        transform.DORotate(new Vector3(0, 0, 400), 1f);
        isActiveForce = false;
    }
}
