using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrack : MonoBehaviour
{
    public string Path;

    public void PlayOneShot(string path)
    {
        AudioManager.Instance.PlayTrack(path, this.transform.position);
    }
}
