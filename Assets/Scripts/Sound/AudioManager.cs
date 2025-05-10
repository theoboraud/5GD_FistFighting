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
            PlayMusic();
        }
    }

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
        GEventCenter.Subscribe(GameEvent.OnGameReset, OnGameReset);
        GEventCenter.Subscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Subscribe(GameEvent.OnShowScore, OnShowScore);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, OnGameReset);
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Unsubscribe(GameEvent.OnShowScore, OnShowScore);
    }

    public void PlayMusic()
    {
        // musicRef.Play();
    }

    public void StopMusic()
    {
        // musicRef.Stop();
    }

    public void PlayWinSound()
    {
        //  WinSound.Play();
    }

    public void StopWinSound()
    {
        //WinSound.Stop();
    }

    public void ChangeParam(float _value)
    {
        //ParamRef.value = _value;
        //ParamRef.TriggerParameters();
    }

    public void PlayTrack(string eventPath, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, position);
    }

    private void OnShowScore()
    {
        PlayWinSound();
        ChangeParam(1);
    }

    private void OnGameReset()
    {
        StopWinSound();
        StopMusic();
    }
    private void OnSceneLoad(GameScene _scene)
    {
        switch (_scene)
        {
            case GameScene.Lobby:
                ChangeParam(1);
                break;
            case GameScene.Playable:
                ChangeParam(2);
                break;
            case GameScene.Intro:
                break;
            case GameScene.Outro:
                StopMusic();
                break;
            default:
                ChangeParam(0);
                break;
        }
    }
}
