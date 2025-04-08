using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Unity.Burst.Intrinsics;
using System.Linq;
using Zenject.SpaceFighter;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class ArmChecker : MonoBehaviour
{
    private List<Player> contactPlayers = new List<Player>();

    [Header("Reference")]
    private Player _player;
    private ArmController _armController;
    private PlayerFeedbackManager _playerFeedbackManager;
    private PlayerController _playerController;
    public ArmAnimationController anim;
    public bool Cooldown = false;
    public bool Holding = false;
    private bool bIsExtend = false;
    private bool bIsHit = false; //Is hit object during extend
    public SpriteRenderer _renderer;

    private float cooldown_timer;
    public float holding_timer;

    // Duration of collision scan during extend
    private float scanDuration = 0.1f; //Nearly smaller than half of anim extend arm times. for beter simulate hit feedback
    // size of box scan
    private Vector2 scanBoxSize = new Vector2(0.8f, 0.5f);
    // Hit colliders
    private HashSet<Collider2D> HitColliders = new HashSet<Collider2D>();

    private void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        _armController = GetComponentInParent<ArmController>();
        _playerFeedbackManager = GetComponentInParent<PlayerFeedbackManager>();
        _playerController = GetComponentInParent<PlayerController>();
    }

    /// <summary>
    ///Init arm on player dead & player start init
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
        if (Cooldown)
        {
            cooldown_timer += Time.deltaTime;
            if (cooldown_timer >= GameManager.Instance.ParamData.PARAM_Player_ArmCooldown)
            {
                StopEverything();
            }
        }
        else if (Holding)
        {
            holding_timer += Time.deltaTime;
            anim.PlayHoldAnimation();
            if (holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
            {
                holding_timer = GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime;
                anim.PlayHoldMaxAnimation();
            }
        }
    }

    /// <summary>
    /// Behaviour on arm extend, call from arm controller
    /// </summary>
    public void OnArmExtend()
    {
        if (Cooldown) return;
        //Declenchement animation
        float ArmScaleFactor = GetPrioPoints();
        _renderer.transform.localScale = new Vector3
            (Mathf.Lerp(1, 1.3f, ArmScaleFactor / (3)),
            Mathf.Lerp(1, 1.3f, ArmScaleFactor / (3)));

        anim.PlayAnimation();
        Cooldown = true;
        _player.VoiceController.StopHold(); //Might change with a event

        bIsExtend = true;

        //ArmClash Check //TODO
        StartCoroutine(SweepDetection());
    }

    /// <summary>
    /// Call at the end of arm extend, calculation interact between players
    /// //TODO
    /// </summary>
    private void EndExtension()
    {
        bIsExtend = false;

        //Calculation arm clash //TODO
    }


    /// <summary>
    /// Scan arm contact area after arm extend
    /// </summary>
    public IEnumerator SweepDetection()
    {
        float detectDistance = 2f; //Disrance for sweep detection (from arm root to arm end)
        Vector2 startPoint = (Vector2)transform.position;
        Vector2 endPoint = (Vector2)transform.position - (Vector2)transform.up * detectDistance;
        Vector2 direction = (endPoint - startPoint).normalized;
        float distance = Vector2.Distance(startPoint, endPoint);
        HitColliders.Clear();
        float elapsed = 0f;
        bIsHit = false;

        while (elapsed < scanDuration)
        {
            if (!bIsExtend) break; //If player get hit we will stop extend arm immediately
            RaycastHit2D[] hits = Physics2D.BoxCastAll(startPoint, scanBoxSize, transform.eulerAngles.z, direction, distance);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null || HitColliders.Contains(hit.collider))
                    continue;
                string tag = hit.collider.tag;
                switch (tag)
                {
                    case "DynamicEnvironment":
                        LaunchForeignObject(hit.collider.GetComponent<Rigidbody2D>());
                        OnHitObject(hit.collider);
                        break;
                    case "Player":
                        Player hitPlayer = hit.collider.GetComponent<Player>();
                        if (hitPlayer != null && hitPlayer != _player)
                        {
                            LaunchForeignPlayer(hitPlayer);
                            OnHitObject(hit.collider);
                            //Stock player for arm clash check(TODO)
                        }
                        break;
                    case "StaticGround":
                        LaunchThisAvatarFromGround();
                        OnHitObject(hit.collider);
                        break;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        //If we extend arm in air and hit noting
        if (!bIsHit)
        {
            //Air dash
            LaunchThisAvatarFromAir();
        }
        EndExtension();
    }

    /// <summary>
    /// If arm hit an object (dynamic obj, player or environment)
    /// </summary>
    private void OnHitObject(Collider2D _collider)
    {
        bIsHit = true;
        HitColliders.Add(_collider);
        Debug.Log("Detected: " + _collider.name + " with tag: " + tag);
    }

    /// <summary>
    /// Init arm state
    /// </summary>
    public void StopEverything()
    {
        Holding = false;
        Cooldown = false;
        cooldown_timer = 0;
        holding_timer = 0;
        anim.StopAnimation();
        bIsExtend = false;
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
    /// Give a back force to player self when he hit Environment
    /// </summary>
    private void LaunchThisAvatarFromGround()
    {
        _player.GetPlayerController().AirPushFactor = 1f;

        _player.GetPlayerController().GetRB().linearVelocity = Vector2.zero;
        _player.GetPlayerController().GetRB().angularVelocity = 0;

        _player.GetPlayerController().GetRB().AddForce
            (transform.up *
            GameManager.Instance.ParamData.PARAM_Player_ArmGroundForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Movement *
            (holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);
        //Debug.Log(Arms[i].holding_timer);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.up, 2.1f);
        _playerFeedbackManager.SpawnEnvHitVFX
            (ray.point,
            Quaternion.AngleAxis(transform.rotation.eulerAngles.z,
            Vector3.forward));
        if (holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
        {
            _playerFeedbackManager.SpawnChargedHit
            (ray.point,
            Quaternion.AngleAxis(transform.rotation.eulerAngles.z,
            Vector3.forward));
        }
    }

    /// <summary>
    ///Give force to dynamic object hits
    /// </summary>
    private void LaunchForeignObject(Rigidbody2D _otherObj)
    {
        _otherObj.AddForce
            (-transform.up *
            GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Hit *
            (holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);

        ShowHitFeedback();
    }

    /// <summary>
    /// Give force to other player hits
    /// </summary>
    /// <param name="_otherPlayer"></param>
    private void LaunchForeignPlayer(Player _otherPlayer)
    {
        _otherPlayer.GetPlayerController().GetRB().linearVelocity = Vector2.zero;
        _otherPlayer.GetPlayerController().GetRB().angularVelocity = 0;
        _otherPlayer.GetPlayerController().GetRB().AddForce
            (-transform.up *
            GameManager.Instance.ParamData.PARAM_Player_ArmHitForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Hit *
            (holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);
        _otherPlayer.Hit();
        _playerFeedbackManager.LastPlayerHit = _otherPlayer;

        ShowHitFeedback();
    }

    /// <summary>
    /// Spawn feedback if we hit other dynamic object or player
    /// </summary>
    private void ShowHitFeedback()
    {

        int strength = (int)Mathf.Lerp(0, 2, GetPrioPoints() / (3));

        _playerFeedbackManager.SpawnPlayerHitVFX
            (Mathf.Clamp(strength, 0, 2), transform.position + transform.up * -2,
            Quaternion.AngleAxis(90 + transform.rotation.eulerAngles.z,
            Vector3.forward));

        if (holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
        {
            _playerFeedbackManager.SpawnChargedHit
            (transform.position + transform.up * -2,
            Quaternion.AngleAxis(90 + transform.rotation.eulerAngles.z,
            Vector3.forward));
        }
    }

    /// <summary>
    /// If the player launch arm in air and hit nothing, he will get a small impulse(AirDash)
    /// </summary>
    private void LaunchThisAvatarFromAir()
    {
        // If the player has already reached the maximum number of jumps in the air, he cannot jump anymore until we reaches the ground
        _playerController.AirPushFactor -= 0.01f;
        float _maxAirPushFactor = 1f - (GameManager.Instance.ParamData.PARAM_Player_AirControlJumpNumber * 0.01f);
        if (_playerController.AirPushFactor < _maxAirPushFactor)
        {
            _playerController.AirPushFactor = 0f;
        }
        // Only reset the velocity if the player can jump
        else
        {
            _playerController.GetRB().linearVelocity *= GameManager.Instance.ParamData.PARAM_Player_VelocityResetFactor;
            _playerController.GetRB().angularVelocity *= GameManager.Instance.ParamData.PARAM_Player_VelocityResetFactor;
        }

        _playerController.GetRB().AddForce
            (transform.up *
             _playerController.AirPushFactor *
            GameManager.Instance.ParamData.PARAM_Player_AirControlForce *
            Mathf.Clamp(GameManager.Instance.ParamData.PARAM_Player_ForceIncreaseFactor_Movement *
            (holding_timer / GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime), 1, 2),
            ForceMode2D.Impulse);

        if (_playerController.AirPushFactor > 0f)
        {
            _playerFeedbackManager.SpawnAirDashVFX
                (transform.position + transform.up * -2,
                Quaternion.AngleAxis(90 + transform.rotation.eulerAngles.z,
                Vector3.forward));
            if (holding_timer >= GameManager.Instance.ParamData.PARAM_Player_MaxTriggerHoldTime)
            {
                _playerFeedbackManager.SpawnChargedHit
                (transform.position + transform.up * -2,
                Quaternion.AngleAxis(90 + transform.rotation.eulerAngles.z,
                Vector3.forward));
            }
        }
    }

    /// <summary>
    /// Add contact players
    /// </summary>
    private void AddContactPlayer(Player _otherPlayer)
    {
        if (_otherPlayer.IsInvincible()) return;
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
    /// Clear all contact data
    /// </summary>
    private void ClearContactData()
    {
        contactPlayers.Clear();
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

    #endregion

    /// <summary>
    /// Debug Draw Scan area
    /// </summary>
    private void DrawScanArea(Vector2 startPoint, Vector2 size, float angle, Vector2 direction, float distance)
    {
        Vector2 boxCenter = startPoint + direction * (distance * 0.5f);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 halfSize = size * 0.5f;

        Vector3 center3 = new Vector3(boxCenter.x, boxCenter.y, 0f);

        Vector3 topLeft = center3 + rotation * new Vector3(-halfSize.x, halfSize.y, 0f);
        Vector3 topRight = center3 + rotation * new Vector3(halfSize.x, halfSize.y, 0f);
        Vector3 bottomRight = center3 + rotation * new Vector3(halfSize.x, -halfSize.y, 0f);
        Vector3 bottomLeft = center3 + rotation * new Vector3(-halfSize.x, -halfSize.y, 0f);

        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);
    }
}
