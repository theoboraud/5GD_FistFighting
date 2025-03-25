using UnityEngine;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Manage all interactions of player&Player, player&Environnement 
/// </summary>
public class InteractionManager:MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================
    [System.NonSerialized] public static InteractionManager Instance;               // Singleton reference

    // #endregion

    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init singleton, references and variables
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // #endregion

    // #region ==================== Class FUNCTIONS ====================
    /// <summary>
    /// Handle situation on arm clash between 2 players
    /// </summary>
    /// <param name="_armPlayer1"></param>
    /// <param name="_armPlayer2"></param>
    public void ArmClash(ArmChecker _armPlayer1, ArmChecker _armPlayer2)
    {
        int player1Points = 0;
        int player2Points = 0;

        // Compare FrameStack
        CompareStats(_armPlayer1.FrameStack, _armPlayer2.FrameStack, ref player1Points, ref player2Points, GameManager.Instance.ParamData.PARAM_PRIO_FRAMESTACK);

        // Compare Velocity
        CompareStats(_armPlayer1.GetRB().linearVelocity.magnitude,
                         _armPlayer2.GetRB().linearVelocity.magnitude,
                         ref player1Points, ref player2Points, GameManager.Instance.ParamData.PARAM_PRIO_VELOCITY);

        // Compare Holding Timer
        CompareStats(_armPlayer1.holding_timer, _armPlayer2.holding_timer, ref player1Points, ref player2Points, GameManager.Instance.ParamData.PARAM_PRIO_HOLDFORCE);

        // Compare Air State
        if (_armPlayer1.GetPlayer().IsInAir() && !_armPlayer2.GetPlayer().IsInAir())
        {
            player1Points += GameManager.Instance.ParamData.PARAM_PRIO_AIRSTATE;
        }
        else if (_armPlayer2.GetPlayer().IsInAir() && !_armPlayer1.GetPlayer().IsInAir())
        {
            player2Points += GameManager.Instance.ParamData.PARAM_PRIO_AIRSTATE;
        }

        HandleArmExtension(player1Points, player2Points, _armPlayer1, _armPlayer2);
    }

    /// <summary>
    /// Add score to winner player in state compare 
    /// </summary>
    private void CompareStats(float stat1, float stat2, ref int player1Points, ref int player2Points, int priorityValue)
    {
        if (stat1 > stat2)
        {
            player1Points += priorityValue;
        }
        else if (stat1 < stat2)
        {
            player2Points += priorityValue;
        }
    }

    /// <summary>
    /// Extended arm beside on player's points
    /// </summary>
    private void HandleArmExtension(int player1Points, int player2Points, ArmChecker _armPlayer1, ArmChecker _armPlayer2)
    {
        var armController1 = _armPlayer1.GetArmController();
        var armController2 = _armPlayer2.GetArmController();

        if (player1Points > player2Points)
        {
            armController1.ExtendedArm(armController1.Arms.IndexOf(_armPlayer1));
        }
        else if (player1Points == player2Points)
        {
            armController1.ExtendedArm(armController1.Arms.IndexOf(_armPlayer1));
            armController2.ExtendedArm(armController2.Arms.IndexOf(_armPlayer2));
        }
        else
        {
            armController2.ExtendedArm(armController2.Arms.IndexOf(_armPlayer2));
        }
    }

    ///// <summary>
    /////Compare player's velocity,and decide which player can extended arm
    /////TODO - Sperate in 2 function - compare & extend, call in interaction Manager
    /////Not In Use????
    ///// </summary>
    //public void ComparePlayersVelocity(ArmChecker _armPlayer1, ArmChecker _armPlayer2)
    //{
    //    if (_armPlayer1.GetPlayer().GetPlayerController().GetRB().linearVelocity.magnitude > _armPlayer2.GetPlayer().GetPlayerController().GetRB().linearVelocity.magnitude)
    //    {
    //        _armPlayer1.GetPlayer().GetPlayerController().GetArmController().ExtendedArm(_armPlayer1.GetPlayer().GetPlayerController().GetArmController().Arms.IndexOf(_armPlayer1));
    //    }
    //    else if (_armPlayer2.GetPlayer().GetPlayerController().GetRB().linearVelocity.magnitude > _armPlayer1.GetPlayer().GetPlayerController().GetRB().linearVelocity.magnitude)
    //    {
    //        _armPlayer2.GetPlayer().GetPlayerController().GetArmController().ExtendedArm(_armPlayer2.GetPlayer().GetPlayerController().GetArmController().Arms.IndexOf(_armPlayer2));
    //    }
    //    else if (Mathf.Approximately(_armPlayer2.GetPlayer().GetPlayerController().GetRB().linearVelocity.magnitude, _armPlayer1.GetPlayer().GetPlayerController().GetRB().linearVelocity.magnitude))
    //    {
    //        _armPlayer2.GetPlayer().GetPlayerController().GetArmController().ExtendedArm(_armPlayer2.GetPlayer().GetPlayerController().GetArmController().Arms.IndexOf(_armPlayer2));
    //        _armPlayer1.GetPlayer().GetPlayerController().GetArmController().ExtendedArm(_armPlayer1.GetPlayer().GetPlayerController().GetArmController().Arms.IndexOf(_armPlayer1));
    //    }
    //}
}
