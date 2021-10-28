using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ParamData", menuName = "Tools/ParamData", order = 1)]
public class ParamData : ScriptableObject
{

// ========================================================================================================================================
// ========================================================== PLAYER PARAMETERS ===========================================================
// ========================================================================================================================================


    // #region =================== GLOBAL PARAMETERS ===================

    [Header("PLAYER GLOBAL PARAMETERS")]
    public float PARAM_Player_Mass;
    public float PARAM_Player_GravityScale;
    public float PARAM_Player_LinearDrag;
    public float PARAM_Player_AngularDrag;
    public float PARAM_Player_StunRecoveryTime;

    // #endregion


    // #region ==================== ARM PARAMETERS =====================

    [Header("PLAYER ARM PARAMETERS")]

    public float PARAM_Player_ArmGroundForce;
    public float PARAM_Player_ArmHitForce;
    public float PARAM_Player_ArmExtendingTime;
    public float PARAM_Player_ArmUnextendingTime;
    public float PARAM_Player_ArmCooldown;

    // #endregion


    // #region ================ AIR CONTROL PARAMETERS =================

    [Header("PLAYER AIR CONTROL PARAMETERS")]
    public float PARAM_Player_AirControlForce;
    public float PARAM_Player_AirControlForceLossFactor;

    // #endregion


    // #region ================== ROTATION PARAMETERS ==================

    [Header("PLAYER ROTATION PARAMETERS")]
    public float PARAM_Player_RotationTorqueMin;
    public float PARAM_Player_RotationTorqueMax;
    public float PARAM_Player_RotationForceMin;
    public float PARAM_Player_RotationForceMax;
    public float PARAM_Player_RotationChargeTimeMax;
    public float PARAM_Player_RotationCooldown;

    // #endregion
}
