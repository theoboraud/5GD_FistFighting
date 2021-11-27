using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FaceBehavior : MonoBehaviour
{
    public GameObject[] eyesOpen;
    public GameObject[] eyesClose;

    private SpriteRenderer mouth;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mouth = transform.Find("Mouth").GetComponent<SpriteRenderer>();
        mouth.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            OnHurt();
            Invoke("HurtPassed",3f);
        }
    }
    private void OnHurt()
    {
        mouth.enabled = true;
    }
    private void HurtPassed()
    {
        mouth.enabled = false;
    }
}
