using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDistracted : StateProcess<WolfState>
{
    WolfMovement movement;
    WolfInfo info;
    Animator anim;

    private void Awake()
    {
        movement = GetComponent<WolfMovement>();
        anim = GetComponentInChildren<Animator>();
        info = GetComponent<WolfInfo>();
    }

    private void OnEnable()
    {
        anim.SetBool("Run", true);
        var speed = GetComponent<WolfFlee>().speed;
        movement.MoveTo(info.DecoyPosition, speed, ReachedDecoy);
    }

    private void OnDisable()
    {
        anim.SetBool("Run", false);
        movement.Stop();
    }

    private void ReachedDecoy()
    {
        StateController.SwitchState(WolfState.Roam);
    }
}
