using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDead : StateProcess<WolfState>
{
    WolfInfo info;
    Animator anim;

    private void Awake()
    {
        info = GetComponent<WolfInfo>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        Debug.Log("Dead");
        info.dead = true;
        anim.SetTrigger("Dead");
    }
}
