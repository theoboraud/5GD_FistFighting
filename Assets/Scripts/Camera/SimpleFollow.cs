using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Vector3 Offset;

    private void Update()
    {
        this.transform.position = Target.position + Offset;
    }
}
