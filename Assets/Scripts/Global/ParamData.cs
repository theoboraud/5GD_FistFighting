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
    public float PARAM_Player_VelocityResetFactor;
    public int PARAM_Player_ArmStartupFrame;

    [Header("DEPRECATED")]
    public float PARAM_Player_ArmExtendingTime;
    public float PARAM_Player_ArmUnextendingTime;
    // #endregion


    // #region ==================== HOLD TRIGGER PARAMETERS ======================

    [Header("HOLD TRIGGER PARAMETERS")]

    public float PARAM_Player_MaxTriggerHoldTime;
    public float PARAM_Player_ForceIncreaseFactor_Movement;
    public float PARAM_Player_ForceIncreaseFactor_Hit;

    // #endregion


    // #region ================ AIR CONTROL PARAMETERS =================

    [Header("PLAYER AIR CONTROL PARAMETERS")]
    public float PARAM_Player_AirControlForce;
    public float PARAM_Player_AirControlForceLossFactor;
    public float PARAM_Player_AirControlJumpNumber;

    // #endregion


    // #region ================== ROTATION PARAMETERS ==================

    [Header("PLAYER ROTATION PARAMETERS")]
    public float PARAM_Player_RotationTorque_InAir;
    public float PARAM_Player_RotationForce_InAir;
    public float PARAM_Player_RotationTorque_OnGround;
    public float PARAM_Player_RotationForce_OnGround;
    public bool PARAM_Player_UseFactorForRotation;

    // #endregion

    // #region ================== PRIORITY PARAMETERS ==================
    [Header("PRIORITY POINTS")]
    public int PARAM_PRIO_FRAMESTACK;
    public int PARAM_PRIO_VELOCITY;
    public int PARAM_PRIO_HOLDFORCE;
    public int PARAM_PRIO_AIRSTATE;
    // #endregion
}
