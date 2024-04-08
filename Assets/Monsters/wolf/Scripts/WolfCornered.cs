using UnityEngine;
using UnityEngine.AI;

public class WolfCornered : StateProcess<WolfState>
{
    public float corneredDuration;

    WolfInfo info;
    NavMeshAgent agent;
    Animator anim;

    float corneredStartTime;

    private void Awake()
    {
        info = GetComponent<WolfInfo>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        anim.SetBool("Idle", true);
        corneredStartTime = Time.time;
        info.wasCornered = true;
        agent.updateRotation = false;
    }

    private void OnDisable()
    {
        agent.updateRotation = true;
    }

    private void Update()
    {
        if (info.TargetPlayer != null)
        {
            var diff = info.TargetPlayer.transform.position - transform.position;
            diff.y = 0;
            transform.rotation = Quaternion.LookRotation(diff, Vector3.up);
        }
        if (info.seenByPlayer)
        {
            StateController.SwitchState(WolfState.Flee);
        }
        else if (Time.time > corneredStartTime + corneredDuration)
        {
            info.wasCornered = false;
            StateController.SwitchState(WolfState.Roam);
        }
    }
}
