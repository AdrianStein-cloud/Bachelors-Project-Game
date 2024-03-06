using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererFoundPlayer : StateProcess<WandererState>
{
    WandererMovement movement;
    WandererInfo info;
    Animator anim;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
        GetComponent<WandererSounds>().OnScreamEnd += StartChase;
    }

    private void OnEnable()
    {
        movement.Stop();
        movement.LookAtTarget(info.LastSeenPlayerLocation);
        anim.SetTrigger("Scream");
    }

    void StartChase()
    {
        StateController.SwitchState(WandererState.Chase);
    }
}
