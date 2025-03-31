using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            float totalDuration = ps.main.duration + ps.main.startLifetime.constant;
            Destroy(gameObject, totalDuration);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}