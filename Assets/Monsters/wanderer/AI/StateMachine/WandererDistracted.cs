using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererDistracted : StateProcess<WandererState>
{
    WandererMovement movement;
    WandererInfo info;
    WandererSight sight;
    Animator anim;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        anim = GetComponent<Animator>();
        info = GetComponent<WandererInfo>();
        sight = GetComponent<WandererSight>();
    }

    private void OnEnable()
    {
        anim.SetBool("Chase", true);
        var speed = GetComponent<WandererChase>().speed;
        info.IsChasing = true;
        movement.MoveTo(info.DecoyPosition, speed, ReachedDecoy);
    }

    private void OnDisable()
    {
        anim.SetBool("Chase", false);
        movement.Stop();
    }

    private void ReachedDecoy()
    {
        StateController.SwitchState(WandererState.Roam);
        info.IsChasing = false;
    }

    private void Update()
    {
        if (sight.IsThereBlockingDoor)
        {
            StateController.InterruptWith(WandererState.OpenDoor);
        }
    }
}
