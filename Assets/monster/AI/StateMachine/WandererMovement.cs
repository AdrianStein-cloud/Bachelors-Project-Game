using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WandererInfo))]
public class WandererMovement : MonoBehaviour
{
    [Header("Door")]
    public float doorFindDistance;
    public LayerMask findDoorMask;
    public GameObject dirObject;

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

    public GameObject CheckForBlockingDoor()
    {
        var doorObj = IsDoorInFront();
        if (doorObj != null)
        {
            var door = doorObj.GetComponent<ToggleDoor>();
            bool doorIsInTheWay = info.CurrentRoom != info.DestinationRoom;
            if (doorObj != null & doorIsInTheWay & !door.open) return doorObj;
        }
        return null;


        /*if (door != null && doorToOpen == null && info.CurrentRoom != info.DestinationRoom)
        {
            doorToOpen = door;
            OpenDoor(door);
            return;
        }*/
    }

    public bool HasDestinationBeenReached()
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

    GameObject IsDoorInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(dirObject.transform.position, dirObject.transform.forward, out hit, doorFindDistance, findDoorMask))
        {
            if (hit.transform.CompareTag("Door"))
            {
                return hit.transform.gameObject;
            }
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
