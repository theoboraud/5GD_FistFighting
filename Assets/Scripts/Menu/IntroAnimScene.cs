using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimScene : MonoBehaviour
{
    private void Awake()
    {
        Invoke("EndAnimIntro", 21) ;
    }

    public void EndAnimIntro()
    {

    }
}
