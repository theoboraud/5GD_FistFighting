using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    public GameObject GO_Player;
    private Image image;

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init image variable
    /// </summary>
    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }


    /// <summary>
    ///     Set indicator pos as the player pos
    /// </summary>
    private void Update()
    {
        Vector3 _pos = Camera.main.WorldToScreenPoint(GO_Player.transform.position);
        _pos.z = 0;
        _pos.y += 55f;
        image.transform.position = _pos;
        //this.transform.position = GO_Player.transform.position;
    }

    // #endregion
}
