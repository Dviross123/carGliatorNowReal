using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    void SetBoolFalse(string boolNaME)
    {
        Animator anim= GetComponent<Animator>();
        anim.SetBool(boolNaME, false);
        GetComponent<melee>().canDamage = true;
    }
}
