using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;

public class WandererChase : StateProcess<WandererState>
{
    public float speed;

    [Header("Attack")]
    public int damage;
    public float damageDelay;
    public float cooldown;
    public int angle;
    public int distance;

    WandererMovement movement;
    WandererSight sight;
    WandererInfo info;

    Animator anim;

    bool canAttack = true;


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

        var player = sight.CheckForPlayerInSight(angle, distance);
        if(player != null)
        {
            AttackPlayer(player);
        }

        movement.MoveTo(info.LastSeenPlayerLocation, speed, PlayerLocationReached);
    }

    void PlayerLocationReached()
    {
        //Needs FIX
        //Sometimes it loses sight of the player while the player is right beside or behind him
        if (!sight.IsPlayerInSight)
        {
            LookForPlayer();
        }
    }

    void LookForPlayer()
    {
        StateController.SwitchState(WandererState.Roam);
    }

    void AttackPlayer(GameObject player)
    {
        if(canAttack)
        {
            StartCoroutine(DoAttack(player));
        }
    }

    IEnumerator DoAttack(GameObject player)
    {
        StartCoroutine(AttackCooldown());

        movement.LookAtTarget(info.LastSeenPlayerLocation);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(damageDelay);
        player
            .GetComponent<PlayerHealth>()
            .TakeDamage(damage);

        /*if (DistanceToTarget() <= attackRange)
        {
            target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }*/
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
