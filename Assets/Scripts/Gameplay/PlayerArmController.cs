using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<ArmChecker> Arms = new List<ArmChecker>();

    public void ExtendArm(int i)
    {
        if(Arms[i].Cooldown == false)
        {
            Arms[i].anim.PlayAnimation();
            Arms[i].Cooldown = true;
            if (CheckIfRigidbodyInRange(i))
            {
                LaunchForeignObject(i);
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

    private bool CheckIfRigidbodyInRange(int i)
    {
        bool inRange = false;

        if (Arms[i].rigidbodies.Count > 0 || Arms[i].players.Count > 0) inRange = true;

        return inRange;
    }

    private bool CheckIfEnvironmentInRange(int i)
    {
        bool inRange = false;

        inRange = Arms[i].StaticEnvironmentInRange;

        return inRange;
    }

    private void LaunchThisAvatarFromGround(int i)
    {
        player.RB.AddForce(Arms[i].transform.up * GameManager.Instance.ParamData.PARAM_Player_ArmGroundForce, ForceMode2D.Impulse);
        RaycastHit2D ray = Physics2D.Raycast(Arms[i].transform.position, -Arms[i].transform.up, 2.1f);
        GameManager.Instance.Feedback.SpawnHitVFX(ray.point, Quaternion.AngleAxis(90 + Arms[i].transform.rotation.eulerAngles.z, Vector3.forward));
    }

    private void LaunchThisAvatarFromAir(int i)
    {
        player.RB.AddForce
            (Arms[i].transform.up * 
            GameManager.Instance.ParamData.PARAM_Player_AirControlForce, 
            ForceMode2D.Impulse);
        GameManager.Instance.Feedback.SpawnHitAvatarVFX
            (Arms[i].transform.position + Arms[i].transform.up * -2, 
            Quaternion.AngleAxis(90 + Arms[i].transform.rotation.eulerAngles.z, 
            Vector3.forward));
    }

    private void LaunchForeignObject(int i)
    {
        foreach (var item in Arms[i].rigidbodies)
        {
            item.AddForce
                (-Arms[i].transform.up * 
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce, 
                ForceMode2D.Impulse);
        }
        foreach (var item in Arms[i].players)
        {
            item.RB.AddForce
                (-Arms[i].transform.up * 
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce, 
                ForceMode2D.Impulse);
            item.Hit();
        }
        GameManager.Instance.Feedback.SpawnHitVFX
            (Arms[i].transform.position + Arms[i].transform.up * -2,
            Quaternion.AngleAxis(90 + Arms[i].transform.rotation.eulerAngles.z,
            Vector3.forward));
    }
}
