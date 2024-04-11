using System.Collections;
using UnityEngine;

public class WandererFoundPlayer : StateProcess<WandererState>
{
    public float cooldown;

    WandererMovement movement;
    WandererInfo info;
    Animator anim;
    EnemyVisionInfo visionInfo;

    bool canScream = true;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
        GetComponent<WandererSounds>().OnScreamEnd += StartChase;
        visionInfo = GetComponent<EnemyVisionInfo>();
    }

    private void OnEnable()
    {
        visionInfo.BeginEffect();
        if (canScream)
        {
            StartCoroutine(ScreamCooldown());
            movement.Stop();
            movement.LookAtTarget(info.LastSeenPlayerLocation);
            anim.SetTrigger("Scream");
        }
        else
        {
            StartChase();
        }
    }

    private void OnDisable()
    {
        //StopAllCoroutines();
    }

    void StartChase()
    {
        if (enabled) 
        { 
            StateController.SwitchState(WandererState.Chase);
        }
    }

    IEnumerator ScreamCooldown()
    {
        canScream = false;
        yield return new WaitForSeconds(cooldown);
        canScream = true;
    }
}
