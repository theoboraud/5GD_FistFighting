using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mouth : MonoBehaviour
{
    public GameObject[] eyesOpen;
    public GameObject[] eyesClose;

    private bool isMouthOpen;
    private SpriteRenderer mouth;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mouth = transform.GetComponent<SpriteRenderer>();
        mouth.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isMouthOpen)
            {
                //collision.gameObject.transform.DOMoveY(-10f, 0.3f);
                isMouthOpen = false;
                return;
            }

            mouth.enabled = true;
            isMouthOpen = true;
        }
    }

}
