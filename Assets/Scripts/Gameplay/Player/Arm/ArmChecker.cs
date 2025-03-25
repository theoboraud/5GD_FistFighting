using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Unity.Burst.Intrinsics;
using System.Linq;
using Zenject.SpaceFighter;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ArmChecker : MonoBehaviour
{
    private List<Rigidbody2D> contactObjects = new List<Rigidbody2D>();
    private List<Player> contactPlayers = new List<Player>();
    public bool StaticEnvironmentInRange = false;

    [Header("Reference")]
    private Player _player;
    private ArmController _armController;
    private BoxCollider2D _collider;
    public ArmAnimationController anim;
    public bool Cooldown = false;
    public bool Holding = false;
    public SpriteRenderer _renderer;

    private float cooldown_timer;
    public float holding_timer;

    public int FrameStack = 0;

    private void OnEnable()
    {
	    _player = GetComponentInParent<Player>();
	    _armController = GetComponentInParent<ArmController>();
	    _collider = GetComponent<BoxCollider2D>();
    }
    
    /// <summary>
    ///
    /// </summary>
    public void InitArm()
    {
        ClearContactData();
        StopEverything(); 
    }

    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        if(_player.IsInAir())
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
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
    private void FixedUpdate()
    {
        if (FrameStack > 0)
        {
            FrameStack -= 1;
            if (FrameStack == 0)
            {
	            _armController.ExtendedArm(_armController.Arms.IndexOf(this));
            }
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

    /// <summary>
    ///
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("DynamicEnvironment"))
        {
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            AddContactObject(rigidbody);
        }
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            AddContactPlayer(player);
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
            RemoveContactObject(rigidbody);
        }
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            RemoveContactPlayer(player);
        }

        if(collision.CompareTag("StaticGround"))
        {
            StaticEnvironmentInRange = false;
        }
    }

    /// <summary>
    /// Add contact players
    /// </summary>
    private void AddContactPlayer(Player _otherPlayer)
    {
        if(_otherPlayer.IsInvincible()) return;
        if (!contactPlayers.Contains(_otherPlayer)) contactPlayers.Add(_otherPlayer);
    }

    /// <summary>
    /// Remove contact players
    /// </summary>
    private void RemoveContactPlayer(Player _otherPlayer)
    {
        if (!contactPlayers.Contains(_otherPlayer)) contactPlayers.Remove(_otherPlayer);
    }

    /// <summary>
    /// Add Ridigbody of contact object
    /// </summary>
    private void AddContactObject(Rigidbody2D _otherObjects)
    {
        if (!contactObjects.Contains(_otherObjects)) contactObjects.Add(_otherObjects);
    }

    /// <summary>
    /// Remove Ridigbody of contact object
    /// </summary>
    private void RemoveContactObject(Rigidbody2D _otherObjects)
    {
        if (contactObjects.Contains(_otherObjects)) contactObjects.Remove(_otherObjects);
    }


    /// <summary>
    /// Clear all contact data
    /// </summary>
    private void ClearContactData()
    {
        contactObjects.Clear();
        contactPlayers.Clear();
        StaticEnvironmentInRange = false;
    }

    #region Public Functions
    /// <summary>
    /// Get owner player of this arm
    /// </summary>
    /// <returns></returns>
    public Player GetPlayer()
    {
        return _player;
    }
    public ArmController GetArmController()
    {
        return _armController;
    }

    public Rigidbody2D GetRB()
    {
        return _player.GetPlayerController().GetRB();
    }

    /// <summary>
    /// Get players in contact
    /// </summary>
    public List<Player> GetContactPlayers()
    {
        return contactPlayers;
    }
    /// <summary>
    /// Get players in contact
    /// </summary>
    public List<Rigidbody2D> GetContactObjects()
    {
        return contactObjects;
    }
    /// <summary>
    /// Get the distance to the nearest rigidbody object in contact.
    /// </summary>
    /// <returns>Distance between arm and nearest rigidbodyObject</returns>
    public float GetClosestRigidbodyPosition()
    {
        // Default shotest distance
        float shortestDist = 150f;

        // Check distance in contact objects
        foreach (Rigidbody2D obj in contactObjects)
        {
            float dist = Vector2.Distance(this.transform.position, obj.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
            }
        }
        // Check distance in contact players
        foreach (Player p in contactPlayers)
        {
            float dist = Vector2.Distance(this.transform.position, p.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
            }
        }

        return shortestDist;
    }

    /// <summary>
    /// Handle situation of arm clash
    /// </summary>
    public void CheckArmClash()
    {
        foreach (Player otherPlayer in contactPlayers)
        {
            foreach (var otherArm in otherPlayer.GetPlayerController().GetArmController().Arms)
            {
                if (otherArm.contactPlayers.Contains(_player))
                {
                    //Debug.Log("On Casse des Gueules !!!");
                    //Invoke Arm Clash
                    InteractionManager.Instance.ArmClash(this, otherArm);
                }
            }
        }
    }

    public int GetPrioPoints()
    {
        int prioPoints = 0;

        if (_player.IsInAir())
        {
            prioPoints += 1;
        }
        if (GetRB().linearVelocity.magnitude > 0.2f)
        {
            prioPoints += 1;
        }
        if (holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
        {
            prioPoints += 1;
        }

        return prioPoints;
    }

    /// <summary>
    /// Check if there are rigidbody objects(includ other player) in range of this arm
    /// </summary>
    public bool IsRigidbodyInRange()
    {
        return (contactObjects.Count > 0 || contactPlayers.Count > 0);
    }

    /// <summary>
    ///Check if this arm interact with environment
    /// </summary>
    public bool IsEnvironmentInRange()
    {
        return StaticEnvironmentInRange;
    }

    #endregion
}
