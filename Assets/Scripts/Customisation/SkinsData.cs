using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsData: MonoBehaviour
{
    [Header("Skins")]
    public List<CharacterSkin> CharacterSkins = new List<CharacterSkin>();      // Character skins library


    /// <summary>
    ///     Get the character skin corresponding to the given index
    /// </summary>
    public CharacterSkin GetSkin(int _index)
    {
        return CharacterSkins[_index];
    }
}
