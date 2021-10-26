using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [Header("Status")]
    public CharacterManager character;
    int index;
    private SelectScreenManager scm;
    public bool Validated;
    [Header("Reference Visuals")]
    [SerializeField] SpriteRenderer LeftArrow;
    [SerializeField] SpriteRenderer RightArrow;
    [SerializeField] Text SkinName;
    [SerializeField] GameObject Ready;

    private void Start()
    {
        LeftArrow.flipX = true;
        scm = FindObjectOfType<SelectScreenManager>();
        index = 0;
        if (scm.Skins.Count > 0)
        {
            character = scm.LinkCharacterToSelector(scm.Skins.Count - 1);
            SkinName.text = character.charSkin.Name;
            character.ChangeSkin(Reference.multipleCharacterManager.characterSkinManager.GetSkin(Mathf.Abs(index)));
        }
    }

    public void ChangeSkinLeft(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (character != null && ctx.started)
        {
            index += -1;
            index %= Reference.multipleCharacterManager.characterSkinManager.characterSkins.Length;
            character.ChangeSkin(Reference.multipleCharacterManager.characterSkinManager.GetSkin(Mathf.Abs(index)));
            SkinName.text = character.charSkin.Name;
        }
    }

    public void ChangeSkinRight(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (character != null && ctx.started)
        {
            index += 1;
            index %= Reference.multipleCharacterManager.characterSkinManager.characterSkins.Length;
            character.ChangeSkin(Reference.multipleCharacterManager.characterSkinManager.GetSkin(Mathf.Abs(index)));
            SkinName.text = character.charSkin.Name;
        }
    }

    public void ValidateSkin()
    {
        Validated = true;
        LeftArrow.gameObject.SetActive(false);
        RightArrow.gameObject.SetActive(false);
        SkinName.gameObject.SetActive(false);
        Ready.SetActive(true);
    }
}
