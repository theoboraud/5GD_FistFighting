using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestMovePlateform : MonoBehaviour
{
    private PlayersManager playersManager;
    private void Start()
    {
        MoveRight();

        playersManager = GameObject.FindObjectOfType<PlayersManager>();
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    //Vector3 _pos;
    //    //_pos = collision.transform.position;
    //    //Vector3 _offset;
    //    //_offset = _pos - transform.position;

    //    //collision.transform.position = new Vector3(transform.position.x+_offset.x, transform.position.y+_offset.y,1);
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(this.transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(playersManager.transform);
    }

    private void MoveLeft()
    {
        transform.DOMoveX(-3, 5f).OnComplete(MoveRight);
    }
    private void MoveRight()
    {
        transform.DOMoveX(3, 5f).OnComplete(MoveLeft); ;
    }
}
