using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SkinSelector : MonoBehaviour
{
    [Header("Status")]
    public Player Player;
    int index;
    public bool Validated;

    [Header("Reference Visuals")]
    [SerializeField] SpriteRenderer LeftArrow;
    [SerializeField] SpriteRenderer RightArrow;
    [SerializeField] Text SkinName;
    [SerializeField] GameObject Ready;

    private void Awake()
    {
        LeftArrow.flipX = true;
        index = 0;
        /*if (selectScreenManager.Skins.Count > 0)
        {
            Player = selectScreenManager.LinkCharacterToSelector(selectScreenManager.Skins.Count - 1);
            SkinName.text = Player.CharSkin.Name;
            Player.ChangeSkin(GameManager.Singleton_GameManager.CharacterSkinManager.GetSkin(Mathf.Abs(index)));
        }*/
    }

    public void ChangeSkinLeft()
    {
        index += -1;
        index %= GameManager.Singleton_GameManager.CharacterSkinManager.CharacterSkins.Length;
        Player.ChangeSkin(GameManager.Singleton_GameManager.CharacterSkinManager.GetSkin(Mathf.Abs(index)));
        SkinName.text = Player.CharSkin.Name;
    }

    public void ChangeSkinRight()
    {
        index += 1;
        index %= GameManager.Singleton_GameManager.CharacterSkinManager.CharacterSkins.Length;
        Player.ChangeSkin(GameManager.Singleton_GameManager.CharacterSkinManager.GetSkin(Mathf.Abs(index)));
        SkinName.text = Player.CharSkin.Name;
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
