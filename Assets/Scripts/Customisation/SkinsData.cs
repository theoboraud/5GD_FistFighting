using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsData: MonoBehaviour
{
    [Header("Skins")]
    public List<CharacterSkin> CharacterSkins = new List<CharacterSkin>();

    public CharacterSkin GetRandomSkin()
    {
        print(CharacterSkins.Count);
        return CharacterSkins[Random.Range(0, CharacterSkins.Count)];
    }

    public CharacterSkin GetSkin(int _index)
    {
        return CharacterSkins[_index];
    }
}
