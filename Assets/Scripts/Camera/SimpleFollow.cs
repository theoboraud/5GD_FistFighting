using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Class used by camera to follow the target
/// </summary>
public class SimpleFollow : MonoBehaviour
{
    
    // #region ==================== CLASS VARIABLES ====================

    [SerializeField] Transform Target;      // Target player ref
    [SerializeField] Vector3 Offset;        // Offset for camera targeting

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Move camera position to target position considering the given offset
    /// </summary>
    private void Update()
    {
        this.transform.position = Target.position + Offset;
    }

    // #endregion
}
