using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArmBehavior : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float force;
    [Header("Refs")]
    public PlayerArmExtender Face;
    [Header("Extension Variables")]
    public bool actif = false;
    [SerializeField] Vector2 StartScaleEndScale;
    [SerializeField] float speedExtension;
    [SerializeField] float speedUnextension;
    [Header("Arm Behaviour Events")]
    public UnityEvent OnAppear;
    public UnityEvent OnExtended;
    public UnityEvent OnCollision;
    public UnityEvent OnUnextended;


    public Sprite JetSprite;
    private Sprite armSprite;

    private SpriteRenderer spriteRenderer;

    private float curScale;

    private bool extended = false;
    private bool grounded = false;
    private bool jeted = false;

    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        armSprite = spriteRenderer.sprite;
    }
    public void ExtensionStart()
    {
        actif = true;
        extended = false;
        spriteRenderer.sprite = armSprite;
    }

    private void Update()
    {
        if(actif)
        {
            if (!extended)
                UpdateScale();
            else
            {
                UnextendArm();
            }
        }
    }

    private void OnStopExtension()
    {
        actif = false;
        OnUnextended.Invoke();
    }

    void UpdateScale()
    {
        if(curScale >= StartScaleEndScale.y)
        {
            OnExtended.Invoke();
            if (grounded) {
                Debug.Log("LESSSS GOOOOO!");
                Face.rb.AddForce(this.transform.up * force, ForceMode2D.Impulse);
                grounded = false;

            }
            else if (!jeted)
            {
                spriteRenderer.sprite = JetSprite;
                Face.rb.AddForce(this.transform.up * force*2f, ForceMode2D.Impulse);
                jeted = true;
                Invoke("ReCharge", 2f);
            }
            extended = true;
        }
        this.transform.localScale = new Vector3(1f, curScale, 1f);

        curScale += speedExtension * Time.deltaTime;
    }

    void UnextendArm()
    {
        if(curScale <= StartScaleEndScale.x)
        {
            OnStopExtension();
        }
        this.transform.localScale = new Vector3(1f, curScale, 1f);

        curScale -= speedUnextension * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StaticGround"))
        {
            grounded = true;
            OnCollision.Invoke();
            Face.OnCollision.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }
    private void ReCharge()
    {
        jeted = false;
    }
}
