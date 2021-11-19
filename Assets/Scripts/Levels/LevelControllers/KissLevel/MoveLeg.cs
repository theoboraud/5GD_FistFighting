using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveLeg : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.DORotate(new Vector3 (0,0,360),0.6f);
        collision.rigidbody.AddForce(Vector3.left*10,ForceMode2D.Impulse);
        Invoke("ReturnLeg",1);       
    }
    private void ReturnLeg()
    {
        transform.DORotate(new Vector3(0, 0, 400), 1f);
    }
}
