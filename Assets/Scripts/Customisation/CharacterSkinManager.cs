using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    [Header("Skins")]
    public CharacterSkin[] CharacterSkins;

    public CharacterSkin GetRandomSkin()
    {
        int rand = Random.Range(0, CharacterSkins.Length);
        return CharacterSkins[rand];
    }

    public CharacterSkin GetSkin(int i)
    {
        return CharacterSkins[i];
    }
}
