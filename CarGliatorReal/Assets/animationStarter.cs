using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStarter : MonoBehaviour
{
    public void animationStop()
    {
        Animator a = GetComponent<Animator>();
        a.SetBool("attack", false);

    }
}
