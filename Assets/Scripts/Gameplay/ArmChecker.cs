using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmChecker : MonoBehaviour
{
    public List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
    public List<Player> players = new List<Player>();
    public bool StaticEnvironmentInRange = false;
    [Header("Reference")]
    public ArmAnimationController anim;
    public bool Cooldown = false;

    private float timer;

    private void Update()
    {
        if(Cooldown)
        {
            timer += Time.deltaTime;
            if(timer >= GameManager.Instance.ParamData.PARAM_Player_ArmCooldown)
            {
                Cooldown = false;
                timer = 0;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("DynamicEnvironment"))
        {
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            if(!rigidbodies.Contains(rigidbody)) rigidbodies.Add(rigidbody);
        }
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(!players.Contains(player)) players.Add(player);
        }
        if (collision.CompareTag("StaticGround"))
        {
            StaticEnvironmentInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DynamicEnvironment"))
        {
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            if (rigidbodies.Contains(rigidbody)) rigidbodies.Remove(rigidbody);
        }
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (players.Contains(player)) players.Remove(player);
        }
        if(collision.CompareTag("StaticGround"))
        {
            StaticEnvironmentInRange = false;
        }
    }
}
