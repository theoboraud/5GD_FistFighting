using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global Feedback Manager, handle all clobal feedback
/// ??We have to separate these VFX functions for single players to there own PlayerFeedbackManager//TODO
/// </summary>
public class FeedbackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject Debris;
    [SerializeField] GameObject GroundBounce;

    public void SpawnDebris(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(Debris, position, rotation, this.transform);
    }

    public void SpawnGroundBounce(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(GroundBounce, position, rotation, this.transform);
    }

    public void ShakeCamera(float duration, float amount)
    {
        SimpleCameraShake SCS = FindFirstObjectByType<SimpleCameraShake>();
        SCS.shakeAmount = amount;
        SCS.shakeDuration = duration;
    }

}
