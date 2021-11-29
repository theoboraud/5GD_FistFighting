using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ParamData", menuName = "Tools/ParamData", order = 1)]
public class ParamData : ScriptableObject
{
    // ========================================================================================================================================
    // =========================================================== GAME PARAMETERS ============================================================
    // ========================================================================================================================================

    // #region =================== GAME PARAMETERS ===================

    [Header("GAME PARAMETERS")]
    [Range(1, 10)]
    public int PARAM_Player_Lives;

    // #endregion

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
    public float PARAM_Player_ArmCooldown;

    [Header("OLD PLAYER ARM PARAMETERS")]
    public float PARAM_Player_ArmExtendingTime;
    public float PARAM_Player_ArmUnextendingTime;
    // #endregion


    // #region ==================== HOLD TRIGGER PARAMETERS ======================

    [Header("HOLD TRIGGER PARAMETERS")]

    public float PARAM_Player_MaxTriggerHoldTime;
    public float PARAM_Player_ForceIncreaseFactor;

    // #endregion


    // #region ================ AIR CONTROL PARAMETERS =================

    [Header("PLAYER AIR CONTROL PARAMETERS")]
    public float PARAM_Player_AirControlForce;
    public float PARAM_Player_AirControlForceLossFactor;

    // #endregion


    // #region ================== ROTATION PARAMETERS ==================

    [Header("PLAYER ROTATION PARAMETERS")]
    public float PARAM_Player_RotationTorque_InAir;
    public float PARAM_Player_RotationForce_InAir;
    public float PARAM_Player_RotationTorque_OnGround;
    public float PARAM_Player_RotationForce_OnGround;
    public bool PARAM_Player_UseFactorForRotation;

    // #endregion
}
