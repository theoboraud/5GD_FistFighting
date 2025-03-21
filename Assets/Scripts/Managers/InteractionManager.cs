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
