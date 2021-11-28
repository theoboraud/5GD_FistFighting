using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChildBehavior : MonoBehaviour
{
    private Transform smirk;
    private Transform fart;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        smirk = transform.Find("Smirk");
        smirk.gameObject.SetActive(false);
        fart = transform.Find("Fart");
        InitFart();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DoFart();
            collision.attachedRigidbody.AddForce((collision.transform.position - transform.position) * 10, ForceMode2D.Impulse);
        }
    }
    private void DoFart()
    {
        smirk.gameObject.SetActive(true);
        fart.gameObject.SetActive(true);
        fart.DOScaleX(0.2f, 0.5f).OnComplete(InitFart);
        Invoke("ReturnInit",2f);
    }
    private void InitFart()
    {
        fart.transform.localScale = new Vector3(0, fart.localScale.y, 0);
        fart.gameObject.SetActive(false);
    }
    private void ReturnInit()
    {
        smirk.gameObject.SetActive(false);
    }
}
