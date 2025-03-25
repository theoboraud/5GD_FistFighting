using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject.SpaceFighter;

public class ArmController : MonoBehaviour
{
    private Player _player;
    private PlayerController _playerController;
    private PlayerFeedbackManager _playerFeedbackManager;
    public List<ArmChecker> Arms = new List<ArmChecker>();
    /// <summary>
    /// List of Arms hitting other players
    /// </summary>
    public List<ArmChecker> ArmsHittingPlayers = new List<ArmChecker>();

    private PlayerPhysicState physicState; //Owner player's physic state
    private bool bIsOnHit; //If owner player is on hit
    public void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        _playerController = GetComponentInParent<PlayerController>();
        _playerFeedbackManager = GetComponentInParent<PlayerFeedbackManager>();
        SubsribeEvents();
        InitArms();
    }
    public void OnDisable()
    {
        UnsribeEvents();
    }
    private void SubsribeEvents()
    {
        _player.EventCenter.Subscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);
        _player.EventCenter.Subscribe<int>(PlayerEvent.OnHoldArm, HoldArm);
        _player.EventCenter.Subscribe<int>(PlayerEvent.OnExtendArm, ExtendArm);
        _player.EventCenter.Subscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
    }

    private void UnsribeEvents()
    {
        _player.EventCenter.Unsubscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);
        _player.EventCenter.Unsubscribe<int>(PlayerEvent.OnHoldArm, HoldArm);
        _player.EventCenter.Unsubscribe<int>(PlayerEvent.OnExtendArm, ExtendArm);
        _player.EventCenter.Unsubscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
    }

    private void OnPlayerGameStateChange(PlayerGameState _gameState)
    {
        if (_gameState == PlayerGameState.Dead)
        {
            InitArms(); //Init Arms On player Dead
        }
    }

    /// <summary>
    ///Init all arms
    /// </summary>
    public void InitArms()
    {
        foreach (var arm in Arms)
        {
            arm.InitArm();
        }
    }


    /// <summary>
    ///When player get hit
    /// </summary>
    private void OnPlayerPhysicStateChange(PlayerPhysicState _newPhysicState)
    {
        physicState = _newPhysicState;
        if (_newPhysicState == Enums.PlayerPhysicState.IsHit)
        {
            bIsOnHit = true;
            for (int i = 0; i < Arms.Count; i++)
            {
                Arms[i].StopEverything();
            }
            for (int i = 0; i < ArmsHittingPlayers.Count; i++)
            {
                ArmChecker _arm = ArmsHittingPlayers[i];
                _arm.FrameStack = 0;
            }
            ArmsHittingPlayers.Clear();
        }
        else
        {
            bIsOnHit = false;
        }
    }


    /// <summary>
    ///
    /// </summary>
    public void HoldArm(int i)
    {
        if (!bIsOnHit && Arms[i].Cooldown == false)
        {
            Arms[i].StartHolding();
        }
    }


    /// <summary>
    /// Called when we start to extend the arm
    /// </summary>
    public void ExtendArm(int _armIndex)
    {
        ArmChecker _arm = Arms[_armIndex];

        if (_arm.Cooldown == false && !bIsOnHit)
        {
            //Declenchement animation
            float ArmScaleFactor = _arm.GetPrioPoints();
            _arm._renderer.transform.localScale = new Vector3
                (Mathf.Lerp(1, 1.3f, ArmScaleFactor / (3)),
                Mathf.Lerp(1, 1.3f, ArmScaleFactor / (3)));

            _arm.anim.PlayAnimation();
            _arm.Cooldown = true;
            _player.VoiceController.StopHold();

            // If we can hit a player, start the frame stack
            if (_arm.GetContactPlayers().Count > 0)
            {

                ArmsHittingPlayers.Add(_arm);

                if (_arm.FrameStack == 0) _arm.FrameStack = GameManager.Instance.ParamData.PARAM_Player_ArmStartupFrame;
                _arm.CheckArmClash();
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


    /// <summary>
    ///     Call when the arm is extended (a.k.a. the frame stack has been emptied for this arm)
    /// </summary>
    public void ExtendedArm(int _armIndex)
    {
        ArmChecker arm = Arms[_armIndex];
        arm.FrameStack = 0;

        if (arm.IsRigidbodyInRange() && !arm.IsEnvironmentInRange())
        {
            LaunchForeignObject(_armIndex);
        }
        else if (arm.IsEnvironmentInRange() && arm.IsRigidbodyInRange())
        {
            RaycastHit2D ray = Physics2D.Raycast(arm.transform.position, -arm.transform.up, 2.1f, LayerMask.GetMask("StaticGround"));
            float nearestDis = arm.GetClosestRigidbodyPosition();
            if (Vector2.Distance(this.transform.position, ray.point) < nearestDis)
            {
                LaunchThisAvatarFromGround(_armIndex);
            }
            else
            {
                LaunchForeignObject(_armIndex);
            }
        }
        else if (arm.IsEnvironmentInRange())
        {
            LaunchThisAvatarFromGround(_armIndex);
        }
        else
        {
            if (!_player.GetPlayerController().HoldingTrigger) LaunchThisAvatarFromAir(_armIndex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    private void LaunchThisAvatarFromGround(int _armIndex)
    {
        _player.GetPlayerController().AirPushFactor = 1f;

        _player.GetPlayerController().GetRB().linearVelocity = Vector2.zero;
        _player.GetPlayerController().GetRB().angularVelocity = 0;

        _player.GetPlayerController().GetRB().AddForce
            (Arms[_armIndex].transform.up *
            GameManager.Instance.ParamData.PARAM_Player_ArmGroundForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Movement *
            (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);
        //Debug.Log(Arms[i].holding_timer);
        RaycastHit2D ray = Physics2D.Raycast(Arms[_armIndex].transform.position, -Arms[_armIndex].transform.up, 2.1f);
        GameManager.Instance.Feedback.SpawnHitVFX
            (ray.point,
            Quaternion.AngleAxis(Arms[_armIndex].transform.rotation.eulerAngles.z,
            Vector3.forward));
        if (Arms[_armIndex].holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
        {
            GameManager.Instance.Feedback.SpawnChargedHit
            (ray.point,
            Quaternion.AngleAxis(Arms[_armIndex].transform.rotation.eulerAngles.z,
            Vector3.forward));
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void LaunchThisAvatarFromAir(int _armIndex)
    {
        // If the player has already reached the maximum number of jumps in the air, he cannot jump anymore until we reaches the ground
        _playerController.AirPushFactor -= 0.01f;
        float _maxAirPushFactor = 1f - (GameManager.Instance.ParamData.PARAM_Player_AirControlJumpNumber * 0.01f);
        if (_playerController.AirPushFactor < _maxAirPushFactor)
        {
            _playerController.AirPushFactor = 0f;
        }
        // Only reset the velocity if the player can jump
        else
        {
            _playerController.GetRB().linearVelocity *= GameManager.Instance.ParamData.PARAM_Player_VelocityResetFactor;
            _playerController.GetRB().angularVelocity *= GameManager.Instance.ParamData.PARAM_Player_VelocityResetFactor;
        }

        _playerController.GetRB().AddForce
            (Arms[_armIndex].transform.up *
             _playerController.AirPushFactor *
            GameManager.Instance.ParamData.PARAM_Player_AirControlForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Movement *
            (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);

        if (_playerController.AirPushFactor > 0f)
        {
            GameManager.Instance.Feedback.SpawnHitAvatarVFX
                (Arms[_armIndex].transform.position + Arms[_armIndex].transform.up * -2,
                Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
                Vector3.forward));
            if (Arms[_armIndex].holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
            {
                GameManager.Instance.Feedback.SpawnChargedHit
                (Arms[_armIndex].transform.position + Arms[_armIndex].transform.up * -2,
                Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
                Vector3.forward));
            }
        }
    }


    /// <summary>
    ///Hit arm triggered objects 
    /// </summary>
    private void LaunchForeignObject(int _armIndex)
    {
        foreach (var item in Arms[_armIndex].GetContactObjects())
        {
            if (item != null)
            {
                item.AddForce
                    (-Arms[_armIndex].transform.up *
                    GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
                    Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Hit *
                    (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
                    ForceMode2D.Impulse);
            }
        }

        foreach (var item in Arms[_armIndex].GetContactPlayers())
        {
            item.GetPlayerController().GetRB().linearVelocity = Vector2.zero;
            item.GetPlayerController().GetRB().angularVelocity = 0;
            item.GetPlayerController().GetRB().AddForce
                (-Arms[_armIndex].transform.up *
                GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
                Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Hit *
                (Arms[_armIndex].holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
                ForceMode2D.Impulse);
            item.Hit();
            _playerFeedbackManager.LastPlayerHit = item;
        }

        ArmsHittingPlayers.Remove(Arms[_armIndex]);

        int strength = (int)Mathf.Lerp(0, 2, Arms[_armIndex].GetPrioPoints() / (3));

        GameManager.Instance.Feedback.SpawnPlayerHit
            (Mathf.Clamp(strength, 0, 2), Arms[_armIndex].transform.position + Arms[_armIndex].transform.up * -2,
            Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
            Vector3.forward));

        if (Arms[_armIndex].holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
        {
            GameManager.Instance.Feedback.SpawnChargedHit
            (Arms[_armIndex].transform.position + Arms[_armIndex].transform.up * -2,
            Quaternion.AngleAxis(90 + Arms[_armIndex].transform.rotation.eulerAngles.z,
            Vector3.forward));
        }
    }

}
