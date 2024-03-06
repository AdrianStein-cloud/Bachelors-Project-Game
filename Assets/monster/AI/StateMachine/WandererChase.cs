using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WandererChase : StateProcess<WandererState>
{
    public float speed;

    WandererMovement movement;
    WandererSight sight;
    WandererInfo info;

    Animator anim;


    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        sight = GetComponent<WandererSight>();
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetBool("Chase", true);
    }

    private void OnDisable()
    {
        anim.SetBool("Chase", false);
    }

    private void FixedUpdate()
    {
        if (sight.IsThereBlockingDoor)
        {
            StateController.InterruptWith(WandererState.OpenDoor);
            return;
        }

        movement.MoveTo(info.LastSeenPlayerLocation, speed, PlayerLocationReached);
    }

    void PlayerLocationReached()
    {
        if (!sight.IsPlayerInSight)
        {
            LookForPlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void LookForPlayer()
    {
        StateController.SwitchState(WandererState.Roam);
    }

    void AttackPlayer()
    {
        Debug.LogWarning("Attack not implemented");
    }
}
