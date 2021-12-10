using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [SerializeField] private Player player;
    public List<ArmChecker> Arms = new List<ArmChecker>();
    public List<ArmChecker> ArmsGoingToHit = new List<ArmChecker>();


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
    ///     Called when we start to extend the arm
    /// </summary>
    public void ExtendArm(int _armIndex)
    {
        ArmChecker _arm = Arms[_armIndex];

        if (_arm.Cooldown == false && player.PlayerPhysicState != Enums.PlayerPhysicState.IsHit)
        {
            _arm.anim.PlayAnimation();
            _arm.Cooldown = true;

            // If we can hit a player, start the frame stack
            if (_arm.Players.Count > 0)
            {
                ArmsGoingToHit.Add(_arm);

                if (_arm.FrameStack == 0)
                {
                    _arm.FrameStack = GameManager.Instance.ParamData.PARAM_Player_ArmStartupFrame;
                }

                if (_arm.Players.Count > 0)
                {
                    for (int i = 0; i < _arm.Players.Count; i++)
                    {
                        print("PlayersArmController: i is " + i.ToString());
                        if (_arm.Players[i].PlayerArmController.ArmsGoingToHit.Count > 0)
                        {
                            for (int j = 0; j < _arm.Players[i].PlayerArmController.ArmsGoingToHit.Count; i++)
                            {
                                print("PlayersArmController: j is " + j.ToString());
                                ArmChecker _armPlayerHit = _arm.Players[i].PlayerArmController.ArmsGoingToHit[j];

                                if (_armPlayerHit.Players.Contains(player))
                                {
                                    Debug.Log("On Casse des Gueules !!!");
                                    TimeToSeeWhoWinsThisClash(Arms[i], _armPlayerHit);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _arm.FrameStack = GameManager.Instance.ParamData.PARAM_Player_ArmStartupFrame;
                }
            }
            else
            {
                ExtendedArm(_armIndex);
            }
        }
    }


    /// <summary>
    ///
    /// </summary>
    public void ComparePlayersVelocity(ArmChecker _armPlayer1, ArmChecker _armPlayer2)
    {
        if (_armPlayer1.Player.RB.velocity.magnitude > _armPlayer2.Player.RB.velocity.magnitude)
        {
            _armPlayer1.Player.PlayerArmController.ExtendedArm(_armPlayer1.Player.PlayerArmController.Arms.IndexOf(_armPlayer1));
        }
        else if (_armPlayer2.Player.RB.velocity.magnitude > _armPlayer1.Player.RB.velocity.magnitude)
        {
            _armPlayer2.Player.PlayerArmController.ExtendedArm(_armPlayer2.Player.PlayerArmController.Arms.IndexOf(_armPlayer2));
        }
        else if (_armPlayer2.Player.RB.velocity.magnitude == _armPlayer1.Player.RB.velocity.magnitude)
        {
            _armPlayer2.Player.PlayerArmController.ExtendedArm(_armPlayer2.Player.PlayerArmController.Arms.IndexOf(_armPlayer2));
            _armPlayer1.Player.PlayerArmController.ExtendedArm(_armPlayer1.Player.PlayerArmController.Arms.IndexOf(_armPlayer1));
        }
    }

    public void TimeToSeeWhoWinsThisClash(ArmChecker _armPlayer1, ArmChecker _armPlayer2)
    {
        int player1Points = 0;
        int player2Points = 0;

        if (_armPlayer1.FrameStack < _armPlayer2.FrameStack)
        {
            player1Points += GameManager.Instance.ParamData.PARAM_PRIO_FRAMESTACK;
        }
        else if (_armPlayer1.FrameStack > _armPlayer2.FrameStack)
        {
            player2Points += GameManager.Instance.ParamData.PARAM_PRIO_FRAMESTACK;
        }

        if (_armPlayer1.Player.RB.velocity.magnitude > _armPlayer2.Player.RB.velocity.magnitude)
        {
            player1Points += GameManager.Instance.ParamData.PARAM_PRIO_VELOCITY;
        }
        else if (_armPlayer1.Player.RB.velocity.magnitude < _armPlayer2.Player.RB.velocity.magnitude)
        {
            player2Points += GameManager.Instance.ParamData.PARAM_PRIO_VELOCITY;
        }

        if(_armPlayer1.holding_timer > _armPlayer2.holding_timer)
        {
            player1Points += GameManager.Instance.ParamData.PARAM_PRIO_HOLDFORCE;
        }
        else if(_armPlayer1.holding_timer < _armPlayer2.holding_timer)
        {
            player2Points += GameManager.Instance.ParamData.PARAM_PRIO_HOLDFORCE;
        }

        if (_armPlayer1.Player.PlayerPhysicState == Enums.PlayerPhysicState.InAir && _armPlayer2.Player.PlayerPhysicState == Enums.PlayerPhysicState.InAir)
        {
            player1Points += GameManager.Instance.ParamData.PARAM_PRIO_AIRSTATE;
        }
        else if (_armPlayer2.Player.PlayerPhysicState == Enums.PlayerPhysicState.InAir && _armPlayer1.Player.PlayerPhysicState == Enums.PlayerPhysicState.InAir)
        {
            player2Points += GameManager.Instance.ParamData.PARAM_PRIO_AIRSTATE;
        }

        if (player1Points > player2Points)
        {
            _armPlayer1.Player.PlayerArmController.ExtendedArm(_armPlayer1.Player.PlayerArmController.Arms.IndexOf(_armPlayer1));
        }
        else if (player1Points == player2Points)
        {
            _armPlayer1.Player.PlayerArmController.ExtendedArm(_armPlayer1.Player.PlayerArmController.Arms.IndexOf(_armPlayer1));
            _armPlayer2.Player.PlayerArmController.ExtendedArm(_armPlayer2.Player.PlayerArmController.Arms.IndexOf(_armPlayer2));
        }
        else if (player2Points>player1Points)
        {
            _armPlayer2.Player.PlayerArmController.ExtendedArm(_armPlayer2.Player.PlayerArmController.Arms.IndexOf(_armPlayer2));
        }
    }

    /// <summary>
    ///     Call when the arm is extended (a.k.a. the frame stack has been emptied for this arm)
    /// </summary>
    public void ExtendedArm(int _armIndex)
    {
        Arms[_armIndex].FrameStack = 0;
        if (CheckIfRigidbodyInRange(_armIndex) && !CheckIfEnvironmentInRange(_armIndex))
        {
            LaunchForeignObject(_armIndex);
        }
        else if(CheckIfEnvironmentInRange(_armIndex) && CheckIfRigidbodyInRange(_armIndex))
        {
            RaycastHit2D ray = Physics2D.Raycast(Arms[_armIndex].transform.position, -Arms[_armIndex].transform.up, 2.1f, LayerMask.GetMask("StaticGround"));
            if(Vector2.Distance(this.transform.position, ray.point) < Arms[_armIndex].GetClosestRigidbodyPosition())
            {
                LaunchThisAvatarFromGround(_armIndex);
            }
            else
            {
                LaunchForeignObject(_armIndex);
            }
        }
        else if (CheckIfEnvironmentInRange(_armIndex))
        {
            LaunchThisAvatarFromGround(_armIndex);
        }
        else
        {
            if(!player.HoldingTrigger) LaunchThisAvatarFromAir(_armIndex);
        }
    }


    /// <summary>
    ///
    /// </summary>
    private bool CheckIfRigidbodyInRange(int _armIndex)
    {
        bool inRange = false;

        if (Arms[_armIndex].Rigidbodies.Count > 0 || Arms[_armIndex].Players.Count > 0)
        {
            inRange = true;
        }

        return inRange;
    }


    /// <summary>
    ///
    /// </summary>
    private bool CheckIfEnvironmentInRange(int _armIndex)
    {
        bool inRange = false;

        inRange = Arms[_armIndex].StaticEnvironmentInRange;

        return inRange;
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchThisAvatarFromGround(int _armIndex)
    {
        player.AirPushFactor = 1f;

        player.RB.velocity = Vector2.zero;
        player.RB.angularVelocity = 0;

        player.RB.AddForce
            (Arms[_armIndex].transform.up *
            GameManager.Instance.ParamData.PARAM_Player_ArmGroundForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Movement *
            (Arms[_armIndex].holding_timer/GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1,2),
            ForceMode2D.Impulse);
        //Debug.Log(Arms[i].holding_timer);
        RaycastHit2D ray = Physics2D.Raycast(Arms[_armIndex].transform.position, -Arms[_armIndex].transform.up, 2.1f);
        GameManager.Instance.Feedback.SpawnHitVFX
            (ray.point,
            Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
            Vector3.forward));
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchThisAvatarFromAir(int _armIndex)
    {
        // If the player has already reached the maximum number of jumps in the air, he cannot jump anymore until we reaches the ground
        player.AirPushFactor -= 0.01f;
        float _maxAirPushFactor = 1f - (GameManager.Instance.ParamData.PARAM_Player_AirControlJumpNumber * 0.01f);
        if (player.AirPushFactor < _maxAirPushFactor)
        {
            player.AirPushFactor = 0f;
        }
        // Only reset the velocity if the player can jump
        else
        {
            player.RB.velocity *= GameManager.Instance.ParamData.PARAM_Player_VelocityResetFactor;
            player.RB.angularVelocity *= GameManager.Instance.ParamData.PARAM_Player_VelocityResetFactor;
        }

        player.RB.AddForce
            (Arms[_armIndex].transform.up *
            player.AirPushFactor *
            GameManager.Instance.ParamData.PARAM_Player_AirControlForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Movement *
            (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);

        if (player.AirPushFactor > 0f)
        {
            GameManager.Instance.Feedback.SpawnHitAvatarVFX
                (Arms[_armIndex].transform.position + Arms[_armIndex].transform.up * -2,
                Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
                Vector3.forward));
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchForeignObject(int _armIndex)
    {
        foreach (var item in Arms[_armIndex].Rigidbodies)
        {
            item.AddForce
                (-Arms[_armIndex].transform.up *
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
                Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Hit *
                (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
                ForceMode2D.Impulse);
        }

        foreach (var item in Arms[_armIndex].Players)
        {
            item.RB.velocity = Vector2.zero;
            item.RB.angularVelocity = 0;
            item.RB.AddForce
                (-Arms[_armIndex].transform.up *
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
                Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Hit *
                (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
                ForceMode2D.Impulse);
            item.Hit();
            AudioManager.Instance.PlayTrack("event:/Voices/Hurt", item.transform.position);
        }

        ArmsGoingToHit.Remove(Arms[_armIndex]);

        GameManager.Instance.Feedback.SpawnHitVFX
            (Arms[_armIndex].transform.position + Arms[_armIndex].transform.up * -2,
            Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
            Vector3.forward));
    }


    public void IsHit()
    {
        for (int i = 0; i < ArmsGoingToHit.Count; i++)
        {
            ArmChecker _arm = ArmsGoingToHit[i];
            _arm.FrameStack = 0;
            ArmsGoingToHit.Remove(_arm);
        }
    }
}
