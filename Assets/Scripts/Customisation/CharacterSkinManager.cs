using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    [Header("Skins")]
    public CharacterSkin[] characterSkins;

    public CharacterSkin GetRandomSkin()
    {
        int rand = Random.Range(0, characterSkins.Length);
        return characterSkins[rand];
    }

    public CharacterSkin GetSkin(int i)
    {
        return characterSkins[i];
    }
}
