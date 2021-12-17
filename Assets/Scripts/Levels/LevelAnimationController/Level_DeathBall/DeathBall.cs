using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathBall : MonoBehaviour
{
    public Sprite AngrySprite;

    private Sprite initSprite;
    private SpriteRenderer m_spriteRenderer;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        m_spriteRenderer = transform.GetComponent<SpriteRenderer>();
        initSprite = m_spriteRenderer.sprite;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_spriteRenderer.sprite = AngrySprite;
            Invoke("ReturnFace", 2f);
        }
    }
    private void ReturnFace()
    {
        m_spriteRenderer.sprite = initSprite;
    }
}
