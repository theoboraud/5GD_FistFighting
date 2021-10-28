using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Transform portalDestination;
    private void Awake()
    {
        portalDestination = transform.GetChild(0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _go = collision.gameObject;
        if (_go.CompareTag("Player"))
        {
            _go.transform.position = portalDestination.position;
        }
    }
}
