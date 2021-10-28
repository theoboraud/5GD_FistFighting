using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrack : MonoBehaviour
{
    public void PlayOneShot(string path)
    {
        AudioManager.audioManager.PlayTrack(path, this.transform.position);
    }
}
