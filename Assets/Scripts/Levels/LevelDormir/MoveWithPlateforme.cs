using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlateforme : MonoBehaviour
{
    private PlayersManager playersManager;
    private void Start()
    {
        playersManager = GameObject.FindObjectOfType<PlayersManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(playersManager.transform);
        }
    }
}
