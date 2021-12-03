using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public bool PlayOnStart = true;
    [SerializeField] FMODUnity.StudioEventEmitter musicRef;
    public static AudioManager audioManager;

    private void Start()
    {
        if(AudioManager.audioManager == null)
        {
            AudioManager.audioManager = this;
        }
        else
        {
            Destroy(this);
        }
        if(PlayOnStart)
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

    public void ChangeParam(float _value)
    {
        musicRef.SetParameter(musicRef.Params[0].ID, _value);
    }

    public void PlayTrack(string eventPath, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, position);
    }
}
