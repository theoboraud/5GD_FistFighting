using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("References")]
    public CharacterSkin charSkin;
    [SerializeField] SpriteRenderer face;
    [SerializeField] SpriteRenderer[] Arm;
    public Rigidbody2D RB;

    private void Awake()
    {
        if(Reference.multipleCharacterManager != null)
        {
            Reference.multipleCharacterManager.AddCharacter(this);
            this.transform.parent = Reference.multipleCharacterManager.transform;
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

    public void ChangeSkin(CharacterSkin charSkin)
    {
        this.charSkin = charSkin;
        Init();
    }
}
