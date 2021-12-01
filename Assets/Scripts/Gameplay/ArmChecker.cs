using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmChecker : MonoBehaviour
{
    public List<Rigidbody2D> Rigidbodies = new List<Rigidbody2D>();
    public List<Player> Players = new List<Player>();
    public bool StaticEnvironmentInRange = false;
    [Header("Reference")]
    [SerializeField] private Player player;
    [SerializeField] BoxCollider2D collider;
    public ArmAnimationController anim;
    public bool Cooldown = false;
    public bool Holding = false;

    private float cooldown_timer;
    public float holding_timer;


    /// <summary>
    ///
    /// </summary>
    public void Init()
    {
        Rigidbodies.Clear();
        Players.Clear();
        StaticEnvironmentInRange = false;
        StopEverything();
    }


    /// <summary>
    ///
    /// </summary>
    private void Start()
    {
        StopEverything();
    }


    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        if(player.PlayerPhysicState == Enums.PlayerPhysicState.InAir)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
        if(Cooldown)
        {
            cooldown_timer += Time.deltaTime;
            if (cooldown_timer >= GameManager.Instance.ParamData.PARAM_Player_ArmCooldown)
            {
                StopEverything();
            }
        }
        else if(Holding)
        {
            holding_timer += Time.deltaTime;
            anim.PlayHoldAnimation();
            if(holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
            {
                holding_timer = GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime;
                anim.PlayHoldMaxAnimation();
            }
        }
        else if(holding_timer == 0)
        {
            StopEverything();
        }
    }


    /// <summary>
    ///
    /// </summary>
    public void StopEverything()
    {
        Holding = false;
        Cooldown = false;
        cooldown_timer = 0;
        holding_timer = 0;
        anim.StopAnimation();
    }


    /// <summary>
    ///
    /// </summary>
    public void StartHolding()
    {
        Holding = true;
        holding_timer = 0;
    }


    public float GetClosestRigidbodyPosition()
    {
        float shortestDist = 150f;
        if(Rigidbodies.Count > 0)
        {
            for (int i = 0; i < Rigidbodies.Count; i++)
            {
                if(Vector2.Distance(this.transform.position, Rigidbodies[i].transform.position) < shortestDist)
                {
                    shortestDist = Vector3.Distance(this.transform.position, Rigidbodies[i].transform.position);
                }
            }
        }
        if(Players.Count > 0)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Vector2.Distance(this.transform.position, Players[i].transform.position) < shortestDist)
                {
                    shortestDist = Vector3.Distance(this.transform.position, Players[i].transform.position);
                }
            }
        }
        return shortestDist;
    }

    /// <summary>
    ///
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("DynamicEnvironment"))
        {
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            if(!Rigidbodies.Contains(rigidbody)) Rigidbodies.Add(rigidbody);
        }
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(!Players.Contains(player) && player.PlayerGameState != Enums.PlayerGameState.Invincible) Players.Add(player);
        }
        if (collision.CompareTag("StaticGround"))
        {
            StaticEnvironmentInRange = true;
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DynamicEnvironment"))
        {
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            if (Rigidbodies.Contains(rigidbody)) Rigidbodies.Remove(rigidbody);
        }
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (Players.Contains(player)) Players.Remove(player);
        }
        if(collision.CompareTag("StaticGround"))
        {
            StaticEnvironmentInRange = false;
        }
    }
}
