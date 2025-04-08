using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject.SpaceFighter;

public class ArmController : MonoBehaviour
{
    private Player _player;
    private PlayerFeedbackManager _playerFeedbackManager;
    public List<ArmChecker> Arms = new List<ArmChecker>();
    /// <summary>
    /// List of Arms hitting other players
    /// </summary>
    public List<ArmChecker> ArmsHittingPlayers = new List<ArmChecker>();

    private bool bIsOnHit; //If owner player is on hit
    public void OnEnable()
    {
        _player = GetComponent<Player>();
        _playerFeedbackManager = GetComponent<PlayerFeedbackManager>();
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
        if (_newPhysicState == Enums.PlayerPhysicState.IsHit)
        {
            bIsOnHit = true;
            for (int i = 0; i < Arms.Count; i++)
            {
                Arms[i].StopEverything();
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
        if (bIsOnHit) return;
        _arm.OnArmExtend();


        //// If we can hit a player, start the frame stack
        //if (_arm.GetContactPlayers().Count > 0)
        //{

        //    ArmsHittingPlayers.Add(_arm);

        //    if (_arm.FrameStack == 0) _arm.FrameStack = GameManager.Instance.ParamData.PARAM_Player_ArmStartupFrame;
        //    _arm.CheckArmClash();
        //}
        //else
        //{
        //    _arm.FrameStack = GameManager.Instance.ParamData.PARAM_Player_ArmStartupFrame;
        //}
        

    }


    /// <summary>
    ///     Call when the arm is extended (a.k.a. the frame stack has been emptied for this arm)
    /// </summary>
    public void ExtendedArm(int _armIndex)
    {
        ArmChecker arm = Arms[_armIndex];

        //if (arm.IsRigidbodyInRange() && !arm.IsEnvironmentInRange())
        //{
        //    LaunchForeignObject(_armIndex);
        //}
        //else if (arm.IsEnvironmentInRange() && arm.IsRigidbodyInRange())
        //{
        //    RaycastHit2D ray = Physics2D.Raycast(arm.transform.position, -arm.transform.up, 2.1f, LayerMask.GetMask("StaticGround"));
        //    float nearestDis = arm.GetClosestRigidbodyPosition();
        //    if (Vector2.Distance(this.transform.position, ray.point) < nearestDis)
        //    {
        //        LaunchThisAvatarFromGround(_armIndex);
        //    }
        //    else
        //    {
        //        LaunchForeignObject(_armIndex);
        //    }
        //}
        //else
        //{
        //    if (!_player.GetPlayerController().HoldingTrigger) LaunchThisAvatarFromAir(_armIndex);
        //}
    }

}
