using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject hitAvatarVFX;
    [SerializeField] GameObject ExpulsionVFX;

    public void SpawnHitVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitVFX, position, rotation, this.transform);
    }

    public void SpawnHitAvatarVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitAvatarVFX, position, rotation, this.transform);
    }

    public void SpawnExpulsionVFX(Vector3 position)
    {
        Vector3 centerPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);
        Quaternion rotation = Quaternion.AngleAxis(Vector3.Angle(ExpulsionVFX.transform.up - position, (centerPos - position).normalized), Vector3.forward);
        GameObject _go = Instantiate(ExpulsionVFX, position, rotation * ExpulsionVFX.transform.rotation, this.transform);
        _go.transform.rotation = Quaternion.LookRotation(centerPos - position);
    }

    public void ShakeCamera(float duration, float amount)
    {
        SimpleCameraShake SCS = FindObjectOfType<SimpleCameraShake>();
        SCS.shakeAmount = amount;
        SCS.shakeDuration = duration;
    }
}
