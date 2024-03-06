using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WandererInfo))]
public class WandererMovement : MonoBehaviour
{
    [Header("Debug Fields")]
    [SerializeField] Vector3 targetPosition;
    [SerializeField] bool moving = false;

    NavMeshAgent agent;

    WandererInfo info;

    Action onDestinationReached;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        info = GetComponent<WandererInfo>();
    }

    private void Update()
    {
        bool destinationReached = HasDestinationBeenReached();

        if (destinationReached)
        {
            moving = false;
            agent.isStopped = true;
            onDestinationReached?.Invoke();
        }
    }

    public void MoveTo(Vector3 target, float speed, Action onDestinationReached = null)
    {
        moving = true;
        targetPosition = target;
        agent.SetDestination(target);
        agent.speed = speed;
        agent.isStopped = false;

        info.DestinationRoom = GetRoomAtLocation(target);
        this.onDestinationReached = onDestinationReached;
    }

    public void Stop()
    {
        moving = false;
        onDestinationReached = null;
        agent.speed = 0;
        agent.isStopped = true;
        info.DestinationRoom = null;
    }

    public void LookAtTarget(Vector3 target)
    {
        var dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir, transform.up);
    }

    bool HasDestinationBeenReached()
    {
        return moving 
            && !agent.pathPending
            && agent.remainingDistance <= agent.stoppingDistance;
    }

    GameObject GetRoomAtLocation(Vector3 point)
    {
        var hits = Physics.OverlapSphere(point, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Room")) return hit.transform.gameObject;
        }
        return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            info.CurrentRoom = other.gameObject;
        }
    }
}
