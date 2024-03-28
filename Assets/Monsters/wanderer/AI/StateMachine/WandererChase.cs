using System.Collections;
using UnityEngine;

public class WandererChase : StateProcess<WandererState>
{
    public float speed;

    [Header("Attack")]
    public int damage;
    public float damageDelay;
    public float cooldown;
    public int attackStartRange;
    public float attackHitRange;

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
        info.IsChasing = true;
        anim.SetBool("Chase", true);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        anim.SetBool("Chase", false);
        movement.Stop();
        canAttack = true;
    }

    private void Update()
    {
        if (sight.IsThereBlockingDoor)
        {
            StateController.InterruptWith(WandererState.OpenDoor);
            return;
        }

        var player = info.TargetPlayer;
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < attackStartRange)
        {
            AttackPlayer(player);
        }

        movement.MoveTo(info.LastSeenPlayerLocation, speed, PlayerLocationReached);
    }

    void PlayerLocationReached()
    {
        if (!sight.IsPlayerInSight && enabled)
        {
            LookForPlayer();
        }
    }

    void LookForPlayer()
    {
        anim.SetBool("Chase", false);
        info.IsChasing = false;
        StateController.SwitchState(WandererState.LookingForPlayer);
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

        movement.LookAtTarget(player.transform.position);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(damageDelay);

        if (sight.DistanceToTarget(player) <= attackHitRange)
        {
            player
                .GetComponent<PlayerHealth>()
                .TakeDamage(damage);
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
