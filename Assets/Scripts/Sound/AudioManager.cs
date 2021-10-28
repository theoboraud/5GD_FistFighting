using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool PlayOnStart = true;
    [SerializeField] FMODUnity.StudioEventEmitter musicRef;

    private void Start()
    {
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

}
