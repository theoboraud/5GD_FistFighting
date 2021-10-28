using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject hitAvatarVFX;

    public void SpawnHitVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitVFX, position, rotation, this.transform);
    }

    public void SpawnHitAvatarVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitAvatarVFX, position, rotation, this.transform);
    }
}
