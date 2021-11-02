using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplePlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 MinMaxCameraSize = new Vector2(7, 15);
    [SerializeField] private float ZoomFactor;

    private void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,Mathf.Lerp(MinMaxCameraSize.x, MinMaxCameraSize.y, Mathf.Clamp(CalculateGreatestDistance() / ZoomFactor, 0,1)), 0.01f);
        this.transform.position = Vector3.Lerp(this.transform.position, GetAverageCameraPosition(), 0.01f);
    }

    private float CalculateGreatestDistance()
    {
        float distance = 0;
        for (int i = 0; i < PlayersManager.Instance.PlayersAlive.Count; i++)
        {
            for (int j = 0; j < PlayersManager.Instance.PlayersAlive.Count; j++)
            {
                if(i != j && Vector2.Distance(PlayersManager.Instance.PlayersAlive[i].transform.position, PlayersManager.Instance.PlayersAlive[j].transform.position) > distance)
                {
                    distance = Vector2.Distance(PlayersManager.Instance.PlayersAlive[i].transform.position, PlayersManager.Instance.PlayersAlive[j].transform.position);
                }
            }
        }
        return distance;
    }

    private Vector3 GetAverageCameraPosition()
    {
        Vector3 pos = new Vector3();
        if (PlayersManager.Instance.PlayersAlive.Count!=0)
        {
            for (int i = 0; i < PlayersManager.Instance.PlayersAlive.Count; i++)
            {
                pos += PlayersManager.Instance.PlayersAlive[i].transform.position;
            }
            pos /= PlayersManager.Instance.PlayersAlive.Count;
            pos.z = this.cam.transform.position.z;
            return pos;
        }
        else
        {
            pos = Vector3.zero;
            return pos;
        }

    }


}