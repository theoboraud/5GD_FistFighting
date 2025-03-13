using UnityEngine;

public static class GlobalSettings
{
	public static readonly float StunRecoveryTime = GameManager.Instance.ParamData.PARAM_Player_StunRecoveryTime;
	public static readonly float PlayerInvincibility = GameManager.Instance.ParamData.PLAYER_INVINCIBILITY;
	
	public static void ApplyPhysicsSettings(Rigidbody2D rb)
	{
		rb.mass = GameManager.Instance.ParamData.PARAM_Player_Mass;
		rb.gravityScale = GameManager.Instance.ParamData.PARAM_Player_GravityScale;
		rb.linearDamping = GameManager.Instance.ParamData.PARAM_Player_LinearDrag;
		rb.angularDamping = GameManager.Instance.ParamData.PARAM_Player_AngularDrag;
	}
}
