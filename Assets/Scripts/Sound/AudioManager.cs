using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public bool PlayOnStart = true;
    [SerializeField] FMODUnity.StudioEventEmitter musicRef;
    [SerializeField] FMODUnity.StudioEventEmitter WinSound;
    [SerializeField] FMODUnity.StudioGlobalParameterTrigger ParamRef;
    public static AudioManager Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (PlayOnStart)
        {
            musicRef.Play();
        }
    }

    public void PlayMusic()
    {
        musicRef.Play();
    }

    public void StopMusic()
    {
        musicRef.Stop();
    }

    public void PlayWinSound()
    {
        WinSound.Play();
    }

    public void StopWinSound()
    {
        WinSound.Stop();
    }

    public void ChangeParam(float _value)
    {
        ParamRef.value = _value;
        ParamRef.TriggerParameters();
    }

    public void PlayTrack(string eventPath, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, position);
    }
}
