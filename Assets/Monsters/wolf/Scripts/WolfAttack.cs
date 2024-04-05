using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfAttack : StateProcess<WolfState>
{
    public float movespeed;
    public float attackStartRange;
    public int damage;

    WolfStateController controller;
    WolfInfo info;
    WolfMovement movement;
    WolfSounds sounds;
    Animator anim;

    bool attacking;


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
        if (info.seenByPlayer)
        {
            //Change to flee
            controller.SwitchState(WolfState.Flee);
            return;
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
    }

    private void OnEnable()
    {
        Debug.Log("ATTACK");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
