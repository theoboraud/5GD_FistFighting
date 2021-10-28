using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] ParticleSystem ParticleSystem;

    public void StartSystem()
    {
        ParticleSystem.Play();
    }

    public void StopSystem()
    {
        ParticleSystem.Stop();
    }
}
