using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerVoiceController : MonoBehaviour
{
    [SerializeField] Player player;
    //The first is the "Hurt" event
    //Secon is the "Push" event
    //Third is the "Victory" event
    [SerializeField] StudioEventEmitter[] studioEventEmitters;

    public void PlayHurt()
    {
        studioEventEmitters[0].Play();
        studioEventEmitters[0].SetParameter(studioEventEmitters[0].Params[0].ID, player.CharSkin.VoiceParameter);
    }

    public void PlayPush()
    {
        studioEventEmitters[1].Play();
        studioEventEmitters[1].SetParameter(studioEventEmitters[1].Params[0].ID, player.CharSkin.VoiceParameter);
    }

    public void PlayVictory()
    {
        studioEventEmitters[2].Play();
        studioEventEmitters[2].SetParameter(studioEventEmitters[2].Params[0].ID, player.CharSkin.VoiceParameter);
    }

    public void PlayHold()
    {
        studioEventEmitters[3].Play();
    }

    public void StopHold()
    {
        studioEventEmitters[3].Stop();
    }
}
