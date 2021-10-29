using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    private bool isBlow = false;
    private Rigidbody2D rigidbody2D;
    private void FixedUpdate()
    {
        if ( isBlow&&rigidbody2D != null )
        {
            rigidbody2D.AddForce(-this.transform.up * 5f, ForceMode2D.Impulse);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigidbody2D = collision.GetComponent<Rigidbody2D>();
        }
        isBlow = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isBlow = false;
    }
}

