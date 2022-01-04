using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonalisaBehavier : MonoBehaviour
{
    private Transform original;
    private Transform hitOnce;
    private Transform hairMess;
    private Transform noHair;
    private Transform angry;
    private Transform helmet;

    private GameObject activeGO;

    private bool isEnter;

    private int enterCount;

    void Start()
    {
        original = transform.Find("Original");
        hitOnce = transform.Find("HitOnce");
        hairMess = transform.Find("HairMess");
        noHair = transform.Find("NoHair");
        angry = transform.Find("Angry");
        helmet = transform.Find("Helmet");

        isEnter = false;

        enterCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnter)
        {
            enterCount++;
            isEnter = true;
            //Debug.Log(isEnter);

            switch (enterCount)
            {
                case 1:
                    OnHitOnce();
                    break;
                case 2:
                    OnHitOnce();
                    break;
                case 3:
                    OnHitOnce();
                    break;
                case 4:
                    OnHairMess();
                    break;
                case 5:
                    OnHairMess();
                    break;
                case 6:
                    OnNoHair();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnHitOnce()
    {
        original.gameObject.SetActive(false);
        activeGO = hitOnce.gameObject;
        activeGO.SetActive(true);

        Invoke("ReturnToOriginal", 1f);
    }
    private void OnHairMess()
    {
        original.gameObject.SetActive(false);
        activeGO = hairMess.gameObject;
        activeGO.SetActive(true);

        Invoke("ReturnToOriginal", 2f);
    }

    private void OnNoHair()
    {
        original.gameObject.SetActive(false);
        noHair.gameObject.SetActive(true);

        Invoke("OnAngry",2f);
    }
    private void OnAngry()
    {
        noHair.gameObject.SetActive(false);
        angry.gameObject.SetActive(true);
        Invoke("OnHelmet",2f);
    }
    private void OnHelmet()
    {
        angry.gameObject.SetActive(false);
        helmet.gameObject.SetActive(true);
    }


    private void ReturnToOriginal()
    {
        if(activeGO!=null) activeGO.SetActive(false);

        original.gameObject.SetActive(true);

        isEnter = false;
    }

}
