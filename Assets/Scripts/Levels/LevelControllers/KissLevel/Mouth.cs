using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mouth : MonoBehaviour
{
    public bool IsLeftMouth;
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
            if (isMouthOpen && IsLeftMouth)
            {
                collision.gameObject.transform.DOMoveX(-5, 0.3f);
                isMouthOpen = false;
                return;
            }
            else if (isMouthOpen && !IsLeftMouth)
            {
                collision.gameObject.transform.DOMoveX(17, 0.3f);
                isMouthOpen = false;
                return;
            }
            mouth.enabled = true;
            isMouthOpen = true;
        }
    }

}
