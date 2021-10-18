using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("References")]
    public CharacterSkin charSkin;
    [SerializeField] SpriteRenderer face;
    [SerializeField] SpriteRenderer[] Arm;

    private void Awake()
    {
        if(Reference.multipleCharacterManager != null)
        {
            Reference.multipleCharacterManager.AddCharacter(this);
            charSkin = Reference.multipleCharacterManager.characterSkinManager.GetRandomSkin();
            Init();
        }
    }

    private void Init()
    {
        face.sprite = charSkin.Face;
        for(int i = 0; i < Arm.Length; i++)
        {
            Arm[i].sprite = charSkin.Arms;
        }
    }
}
