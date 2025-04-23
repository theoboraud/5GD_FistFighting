using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerSelector : MonoBehaviour
{
    [Header("Status")]
    [System.NonSerialized] public Player player;
    [System.NonSerialized] public bool Validated;

    [Header("InGameUI References")]
    [SerializeField] private SpriteRenderer LeftArrow;
    [SerializeField] private SpriteRenderer RightArrow;
    [SerializeField] private Text SkinName;
    [SerializeField] private GameObject Ready;

    private int index;

    private void Awake()
    {
        LeftArrow.flipX = true;
    }

    public void Init()
    {
        if (player != null)
        {
            index = Random.Range(0, PlayersManager.Instance.SkinsData.CharacterSkins.Count);
            player.ChangeSkin(PlayersManager.Instance.SkinsData.GetSkin(Mathf.Abs(index)));
            SkinName.text = player.GetSkin().Name;
        }
    }

    public void ChangeSkinLeft()
    {
        index += -1;
        index %= PlayersManager.Instance.SkinsData.CharacterSkins.Count;
        player.ChangeSkin(PlayersManager.Instance.SkinsData.GetSkin(Mathf.Abs(index)));
        SkinName.text = player.GetSkin().Name;
    }

    public void ChangeSkinRight()
    {
        index += 1;
        index %= PlayersManager.Instance.SkinsData.CharacterSkins.Count;
        player.ChangeSkin(PlayersManager.Instance.SkinsData.GetSkin(Mathf.Abs(index)));
        SkinName.text = player.GetSkin().Name;
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
