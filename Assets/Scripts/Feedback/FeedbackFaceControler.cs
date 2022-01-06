using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackFaceControler : MonoBehaviour
{
    public bool CanShake = true;
    private bool IsShaking = false;
    private float shaketime = 0.15f;

    public void ShakeFace()
    {
        if (CanShake && !IsShaking)
        {
            IsShaking = true;
            transform.DOShakeScale(shaketime,0.4f,3,45,true);
            Invoke("NotShaking", shaketime);
        }
    }

    private void NotShaking()
    {
        IsShaking = false;
    }

}
