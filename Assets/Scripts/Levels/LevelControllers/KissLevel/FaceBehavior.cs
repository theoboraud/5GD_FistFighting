using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FaceBehavior : MonoBehaviour
{
    public SpriteRenderer[] EyesOpen;
    public SpriteRenderer[] EyesClose;
    public MoveLeg MoveLeg;

    private SpriteRenderer mouth;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mouth = transform.Find("Mouth").GetComponent<SpriteRenderer>();
        mouth.enabled = false;
        foreach (var eyeClose in EyesClose)
        {
            eyeClose.enabled = false;
        }
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
        foreach (var eyeOpen in EyesOpen)
        {
            eyeOpen.enabled = false;
        }
        foreach (var eyeClose in EyesClose)
        {
            eyeClose.enabled = true;
        }

        MoveLeg.ActiveLegMove();
    }
    private void HurtPassed()
    {
        mouth.enabled = false;
        foreach (var eyeOpen in EyesOpen)
        {
            eyeOpen.enabled = true;
        }
        foreach (var eyeClose in EyesClose)
        {
            eyeClose.enabled = false;
        }
    }
}
