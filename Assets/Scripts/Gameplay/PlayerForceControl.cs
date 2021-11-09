using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceControl : MonoBehaviour
{
    [SerializeField] private Player player;
    private float currentForce;
    private float timer;

    private void Update()
    {
        if(player.HoldingTrigger)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime);
            currentForce = Mathf.Lerp(1, GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor, timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime);
        }
        else
        {
            timer = 0;
            currentForce = 1;
        }
        player.ForceIncreaseFactor = currentForce;
    }
}
