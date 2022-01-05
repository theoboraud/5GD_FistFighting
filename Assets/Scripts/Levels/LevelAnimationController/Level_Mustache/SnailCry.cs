using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailCry : MonoBehaviour
{
    public GameObject Cry;
    public GameObject SnailEgg;

    private bool isColl = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isColl)
        {
            Cry.SetActive(true);
            ThrowEgg();
            Invoke("StopCry", 1f);
            isColl = true;
        }
    }

    private void ThrowEgg()
    {
        GameObject newEgg= Instantiate(SnailEgg);
        GameObject.Destroy(newEgg,10f);
    }
    private void StopCry()
    {
       Cry.SetActive(false);
        isColl = false;
    }

}
