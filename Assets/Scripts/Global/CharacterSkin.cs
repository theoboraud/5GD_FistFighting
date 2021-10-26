using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Skin", menuName = "CharacterSkin")] -> TODO ASK ALEXANDER WHAT THE HELL IT IS!
public struct CharacterSkin
{
    [Header("General")]
    public string Name;
    public Sprite SpriteFace;
    public Sprite SpriteArm;
}
