using System.Collections;
using UnityEngine;

public class WolfAttack : StateProcess<WolfState>
{
    public float movespeed;
    public float attackStartRange;
    public int damage;
    public float seenRangeAllowHit;

    WolfInfo info;
    WolfMovement movement;
    WolfSounds sounds;
    Animator anim;

    bool attacking;
    bool hasBeenSeen;
    float seenRange;


    private void Awake()
    {
        movement = GetComponent<WolfMovement>();
        info = GetComponent<WolfInfo>();
        sounds = GetComponent<WolfSounds>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if(info.TargetPlayer == null)
        {
            StateController.SwitchState(WolfState.Roam);
            return;
        }

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
        StateController.SwitchState(WolfState.Flee);
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
        yield return new WaitForSeconds(0.5f);

        anim.SetBool("Attack", false);
        attacking = false;

        if (hasBeenSeen)
        {
            //Change to flee
            StateController.SwitchState(WolfState.Flee);
        }
    }

    private void OnEnable()
    {
        hasBeenSeen = false;
        attacking = false;
        seenRange = 0;
    }

    private void OnDisable()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Run", false);
        StopAllCoroutines();
    }
}
