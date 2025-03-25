using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Enums;

public class PlayerVoiceController : MonoBehaviour
{
    [SerializeField] Player _player;
    
    //The first is the "Hurt" event
    //Secon is the "Push" event
    //Third is the "Victory" event
    [SerializeField] StudioEventEmitter[] studioEventEmitters;

    private void Awake()
    {
	    _player = GetComponent<Player>();
    }

    public void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        SubsribeEvents();
    }
    public void OnDisable()
    {
        UnsribeEvents();
    }
    private void SubsribeEvents()
    {
        _player.EventCenter.Subscribe<int>(PlayerEvent.OnHoldArm, PlayHold);
    }

    private void UnsribeEvents()
    {
        _player.EventCenter.Unsubscribe<int>(PlayerEvent.OnHoldArm, PlayHold);
    }

    public void PlayHurt()
    {
        //studioEventEmitters[0].Play();
        //studioEventEmitters[0].SetParameter(studioEventEmitters[0].Params[0].ID, _player.GetSkin().VoiceParameter);
    }

    public void PlayPush()
    {
        //studioEventEmitters[1].Play();
        //studioEventEmitters[1].SetParameter(studioEventEmitters[1].Params[0].ID, _player.GetSkin().VoiceParameter);
    }

    public void PlayVictory()
    {
        //studioEventEmitters[2].Play();
        //studioEventEmitters[2].SetParameter(studioEventEmitters[2].Params[0].ID, _player.GetSkin().VoiceParameter);
    }

    public void PlayHold(int armIndex)
    {
        //studioEventEmitters[3].Play();
    }

    public void StopHold()
    {
        //studioEventEmitters[3].Stop();
    }
}
