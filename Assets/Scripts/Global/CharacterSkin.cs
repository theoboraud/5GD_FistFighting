using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "CharacterSkin")]
public class CharacterSkin : ScriptableObject
{
    [Header("General")]
    public string Name;
    public Sprite SpriteFace;
    public Sprite SpriteArm;
}
