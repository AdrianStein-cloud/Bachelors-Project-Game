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
        var dir = info.LastSeenPlayerLocation - transform.position;
        transform.rotation = Quaternion.LookRotation(dir, transform.up);
        anim.SetTrigger("Scream");
    }

    void StartChase()
    {
        StateController.SwitchState(WandererState.Chase);
    }
}
