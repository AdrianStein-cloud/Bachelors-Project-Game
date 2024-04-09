using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfAttack : StateProcess<WolfState>
{
    public float movespeed;
    public float attackStartRange;
    public int damage;
    public float seenRangeAllowHit;

    WolfStateController controller;
    WolfInfo info;
    WolfMovement movement;
    WolfSounds sounds;
    Animator anim;

    bool attacking;
    bool hasBeenSeen;
    float seenRange;


    private void Awake()
    {
        controller = GetComponent<WolfStateController>();
        movement = GetComponent<WolfMovement>();
        info = GetComponent<WolfInfo>();
        sounds = GetComponent<WolfSounds>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (info.seenByPlayer && !hasBeenSeen)
        {
            hasBeenSeen = true;
            seenRange = Vector3.Distance(transform.position, info.TargetPlayer.transform.position);
            sounds.RunawaySound();

            if (seenRange > seenRangeAllowHit)
            {
                StartCoroutine(WaitBeforeFlee());
                return;
            }
        }

        if (info.canSeePlayer)
        {
            if (Vector3.Distance(transform.position, info.TargetPlayer.transform.position) <= attackStartRange && !attacking)
            {
                Attack();
            }

            if (!attacking)
            {
                anim.SetBool("Run", true);
                movement.MoveTo(info.TargetPlayer.transform.position, movespeed);
            }
        }
        else
        {
            //If on attack and can't see player, go to roam / idle mode.
        }
    }

    IEnumerator WaitBeforeFlee()
    {
        yield return new WaitForSeconds(0.5f);
        controller.SwitchState(WolfState.Flee);
    }

    void Attack()
    {
        StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        attacking = true;
        anim.SetBool("Run", false);
        anim.SetBool("Attack", true);
        sounds.AttackSound();

        var health = info.TargetPlayer.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);

        //wait animation length
        yield return new WaitForSeconds(1f);

        anim.SetBool("Attack", false);
        attacking = false;

        if (hasBeenSeen)
        {
            //Change to flee
            StartCoroutine(WaitBeforeFlee());
        }
    }

    private void OnEnable()
    {
        hasBeenSeen = false;
        seenRange = 0;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
