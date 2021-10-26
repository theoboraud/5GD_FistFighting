using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsData: MonoBehaviour
{
    [Header("Skins")]
    public CharacterSkin[] CharacterSkins;

    public CharacterSkin GetRandomSkin()
    {
        int _rand = Random.Range(0, CharacterSkins.Length);
        return CharacterSkins[_rand];
    }

    public CharacterSkin GetSkin(int _index)
    {
        return CharacterSkins[_index];
    }
}
