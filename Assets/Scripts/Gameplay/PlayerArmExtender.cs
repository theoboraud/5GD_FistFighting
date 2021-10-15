using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerArmExtender : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameObject Arm;
    public Rigidbody2D rb;
    public ArmBehavior[] Arms = new ArmBehavior[4];
    [Header("Events")]
    public UnityEvent OnExtendArm;
    public UnityEvent OnCollision;

    public void ExtendArm(int Arm)
    {
        OnExtendArm.Invoke();
        CreateArm(Arm);
    }

    void CreateArm(int i)
    {
        if(Arms[i].actif == false)
        {
            Arms[i].ExtensionStart();
        }
    }
}
