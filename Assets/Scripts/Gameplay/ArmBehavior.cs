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

    private float curScale;

    private bool extended = false;

    private bool Grounded = false;

    public void ExtensionStart()
    {
        actif = true;
        extended = false;
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
            if (Grounded) {
                Debug.Log("LESSSS GOOOOO!");
                Face.rb.AddForce(this.transform.up * force, ForceMode2D.Impulse);
                Grounded = false;
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
            Grounded = true;
            OnCollision.Invoke();
            Face.OnCollision.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Grounded = false;
    }
}
