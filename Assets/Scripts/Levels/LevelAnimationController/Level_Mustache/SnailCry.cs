using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailCry : MonoBehaviour
{
    public GameObject Cry;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Cry.SetActive(true);
        Invoke("StopCry",1f);
    }
    private void StopCry()
    {
       Cry.SetActive(false);
    }

}
