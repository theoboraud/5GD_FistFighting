using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<ArmChecker> Arms = new List<ArmChecker>();


    /// <summary>
    ///
    /// </summary>
    public void Init()
    {
        for (int i = 0; i < Arms.Count; i++)
        {
            Arms[i].Init();
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        if (player.PlayerPhysicState == Enums.PlayerPhysicState.IsHit)
        {
            for (int i = 0; i < Arms.Count; i++)
            {
                Arms[i].StopEverything();
            }
        }
    }


    /// <summary>
    ///
    /// </summary>
    public void HoldArm(int i)
    {
        if (player.PlayerPhysicState != Enums.PlayerPhysicState.IsHit && Arms[i].Cooldown == false)
            Arms[i].StartHolding();
    }


    /// <summary>
    ///
    /// </summary>
    public void ExtendArm(int i)
    {
        if (Arms[i].Cooldown == false && player.PlayerPhysicState != Enums.PlayerPhysicState.IsHit)
        {
            Arms[i].anim.PlayAnimation();
            Arms[i].Cooldown = true;

            if (CheckIfRigidbodyInRange(i) && !CheckIfEnvironmentInRange(i))
            {
                LaunchForeignObject(i);
            }
            else if(CheckIfEnvironmentInRange(i) && CheckIfRigidbodyInRange(i))
            {
                RaycastHit2D ray = Physics2D.Raycast(Arms[i].transform.position, -Arms[i].transform.up, 2.1f, LayerMask.GetMask("StaticGround"));
                if(Vector2.Distance(this.transform.position, ray.point) < Arms[i].GetClosestRigidbodyPosition())
                {
                    LaunchThisAvatarFromGround(i);
                }
                else
                {
                    LaunchForeignObject(i);
                }
            }
            else if (CheckIfEnvironmentInRange(i))
            {
                LaunchThisAvatarFromGround(i);
            }
            else
            {
                if(!player.HoldingTrigger) LaunchThisAvatarFromAir(i);
            }
        }

    }


    /// <summary>
    ///
    /// </summary>
    private bool CheckIfRigidbodyInRange(int i)
    {
        bool inRange = false;

        if (Arms[i].Rigidbodies.Count > 0 || Arms[i].Players.Count > 0)
        {
            inRange = true;
        }

        return inRange;
    }


    /// <summary>
    ///
    /// </summary>
    private bool CheckIfEnvironmentInRange(int i)
    {
        bool inRange = false;

        inRange = Arms[i].StaticEnvironmentInRange;

        return inRange;
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchThisAvatarFromGround(int i)
    {
        player.AirPushFactor = 1f;

        player.RB.velocity *= 0.75f;
        player.RB.angularVelocity *= 0.75f;

        player.RB.AddForce
            (Arms[i].transform.up *
            GameManager.Instance.ParamData.PARAM_Player_ArmGroundForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor *
            (Arms[i].holding_timer/GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1,2),
            ForceMode2D.Impulse);
        //Debug.Log(Arms[i].holding_timer);
        RaycastHit2D ray = Physics2D.Raycast(Arms[i].transform.position, -Arms[i].transform.up, 2.1f);
        GameManager.Instance.Feedback.SpawnHitVFX
            (ray.point,
            Quaternion.AngleAxis(90 + Arms[i].transform.rotation.eulerAngles.z,
            Vector3.forward));
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchThisAvatarFromAir(int i)
    {
        player.AirPushFactor -= 0.01f;
        if (player.AirPushFactor <= 0.97f)
        {
            player.AirPushFactor = 0f;
        }
        else
        {
            player.RB.velocity /= 2;
            player.RB.angularVelocity /= 2;
        }

        player.RB.AddForce
            (Arms[i].transform.up *
            player.AirPushFactor *
            GameManager.Instance.ParamData.PARAM_Player_AirControlForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor *
            (Arms[i].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);

        GameManager.Instance.Feedback.SpawnHitAvatarVFX
            (Arms[i].transform.position + Arms[i].transform.up * -2,
            Quaternion.AngleAxis(90 + Arms[i].transform.rotation.eulerAngles.z,
            Vector3.forward));
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchForeignObject(int i)
    {
        foreach (var item in Arms[i].Rigidbodies)
        {
            item.AddForce
                (-Arms[i].transform.up *
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
                Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor *
                (Arms[i].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
                ForceMode2D.Impulse);
        }

        foreach (var item in Arms[i].Players)
        {
            item.RB.AddForce
                (-Arms[i].transform.up *
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
                Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor *
                (Arms[i].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
                ForceMode2D.Impulse);
            item.Hit();
        }

        GameManager.Instance.Feedback.SpawnHitVFX
            (Arms[i].transform.position + Arms[i].transform.up * -2,
            Quaternion.AngleAxis(90 + Arms[i].transform.rotation.eulerAngles.z,
            Vector3.forward));
    }
}
