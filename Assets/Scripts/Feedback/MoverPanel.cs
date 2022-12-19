using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoverPanel : MonoBehaviour
{
    public void Start()
    {
        Invoke("CallAnim", 5.5f);
    }

    public void CallAnim()
    {
        transform.DOMoveY(14, 0.3f, false);
        Invoke("Arrivee", 0.3f);
    }

    public void Arrivee()
    {
        transform.DOPunchPosition(Vector3.up*15, 0.25f, 10, 1, false);
        StartCoroutine(WaitingAnim());
    }

    IEnumerator WaitingAnim()
    {
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(3f);
            if(i%2 == 0) { transform.DOPunchRotation(Vector3.forward * 12, 0.2f, 20, 1); } else { transform.DOPunchRotation(Vector3.forward * -12, 0.2f, 20, 1); }
        }
    }
}
