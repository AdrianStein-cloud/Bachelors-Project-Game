using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererStunned : StateInterrupt
{

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetBool("Stunned", true);
    }

    private void OnDisable()
    {
        anim.SetBool("Stunned", false);
    }
}
