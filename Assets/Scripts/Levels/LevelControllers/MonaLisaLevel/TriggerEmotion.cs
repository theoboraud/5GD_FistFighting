using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEmotion : MonoBehaviour
{
    public GameObject MonalisaBehavierGo;
    private SpriteRenderer Image;
    private void Start()
    {
        Image = gameObject.GetComponentInParent<SpriteRenderer>();
        Image.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Image.enabled = true;
            MonalisaBehavierGo.SetActive(false);
            Invoke("CloseImage", 1f);
        }
    }
    private void CloseImage()
    {
        Image.enabled = false;
        MonalisaBehavierGo.SetActive(true);
    }
}
