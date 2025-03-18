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
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject hitAvatarVFX;
    [SerializeField] GameObject ExpulsionVFX;
    [SerializeField] GameObject Debris;
    [SerializeField] GameObject[] HitPlayer;
    [SerializeField] GameObject ChargedHit;
    [SerializeField] GameObject GroundBounce;

    [Header("Variables")]
    private List<GameObject> allVFX = new List<GameObject>();

    public void OnEnable()
    {
        SubsribeEvents();
    }
    public void OnDisable()
    {
        UnsribeEvents();
    }

    private void SubsribeEvents()
    {
        GEventCenter.Subscribe(GameEvent.OnNewGameRound, ResetAllVFX);
        GEventCenter.Subscribe(GameEvent.OnGameReset, ResetAllVFX);
        GEventCenter.Subscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe(GameEvent.OnNewGameRound, ResetAllVFX);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, ResetAllVFX);
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
    }

    public void ResetAllVFX()
    {
        for (int i = 0; i < allVFX.Count; i++)
        {
            Destroy(allVFX[i]);
        }
        
        allVFX.Clear();
    }

    public void SpawnHitVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitVFX, position, rotation, this.transform);
        allVFX.Add(_go);
    }

    public void SpawnHitAvatarVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitAvatarVFX, position, rotation, this.transform);
        allVFX.Add(_go);
    }

    public void SpawnExpulsionVFX(Vector3 position)
    {
        Vector3 centerPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);
        Quaternion rotation = Quaternion.AngleAxis(Vector3.Angle(ExpulsionVFX.transform.up - position, (centerPos - position).normalized), Vector3.forward);
        GameObject _go = Instantiate(ExpulsionVFX, position, rotation * ExpulsionVFX.transform.rotation, this.transform);
        _go.transform.rotation = Quaternion.LookRotation(centerPos - position);
        allVFX.Add(_go);
    }

    public void SpawnChargedHit(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(ChargedHit, position, rotation, this.transform);
        allVFX.Add(_go);
    }

    public void SpawnPlayerHit(int strength, Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(HitPlayer[strength], position, rotation, this.transform);
        allVFX.Add(_go);
    }

    public void SpawnDebris(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(Debris, position, rotation, this.transform);
        allVFX.Add(_go);
    }

    public void SpawnGroundBounce(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(GroundBounce, position, rotation, this.transform);
        allVFX.Add(_go);
    }

    public void ShakeCamera(float duration, float amount)
    {
        SimpleCameraShake SCS = FindFirstObjectByType<SimpleCameraShake>();
        SCS.shakeAmount = amount;
        SCS.shakeDuration = duration;
    }

    private void OnSceneLoad(GameScene _scene)
    {
        if (_scene == GameScene.Outro)
        {
            ResetAllVFX();
        }
    }
}
