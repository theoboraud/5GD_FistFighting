using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float DestroyAfter;
    void Start()
    {
        Invoke("DestroyThis", DestroyAfter);
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
