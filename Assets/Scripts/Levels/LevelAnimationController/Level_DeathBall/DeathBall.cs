using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathBall : MonoBehaviour
{
    public Sprite AngrySprite;

    private Sprite initSprite;
    private SpriteRenderer m_spriteRenderer;
    private DeathBallController m_deathBallController;
    private PlayersManager m_PlayersManager;

    private bool isUpgradeDiff = false;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        m_spriteRenderer = transform.GetComponent<SpriteRenderer>();
        initSprite = m_spriteRenderer.sprite;

        m_PlayersManager = FindObjectOfType<PlayersManager>();
        m_deathBallController = transform.GetComponentInParent<DeathBallController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If kill player
        if (collision.gameObject.CompareTag("Player"))
        {
            m_spriteRenderer.sprite = AngrySprite;
            m_deathBallController.MoveSpeed *=3;
            if (m_PlayersManager.PlayersLives.Count<=2 && !isUpgradeDiff) //If there is less than 2 players lives in current game
            {
                m_deathBallController.UnlockDestinations();//We'll unlock all destinations and change ball's track
                m_deathBallController.MoveSpeed *= 2; //And We'll upgrade move speed to double
                isUpgradeDiff = true;
            }
            Invoke("ReturnFace", 2f);
        }
    }
    private void ReturnFace()
    {
        m_spriteRenderer.sprite = initSprite;
        m_deathBallController.MoveSpeed /= 3;
    }
}
