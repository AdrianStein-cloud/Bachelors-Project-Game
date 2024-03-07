using UnityEngine;

public class WandererSight : MonoBehaviour
{
    public GameObject eyes;

    [Header("Player Sight")]
    public LayerMask findPlayerMask;
    public float angle;
    public float distance;
    public float persitanceDuration = 0.4f;

    [Header("Door Sight")]
    public float doorFindDistance;
    public LayerMask findDoorMask;

    public bool IsPlayerInSight => info.TargetPlayer != null;
    public bool IsThereBlockingDoor => door != null;


    WandererInfo info;

    GameObject target;
    GameObject door;
    float lastSeenPlayerTime;

    private void Awake()
    {
        info = GetComponent<WandererInfo>();

        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        door = CheckForBlockingDoor();
        if(door != null)
        {
            info.DoorToOpen = door;
        }
        info.TargetPlayer = CheckForPlayerInSight(angle, distance);
        if(info.TargetPlayer != null)
        {
            info.LastSeenPlayerLocation = info.TargetPlayer.transform.position;
            lastSeenPlayerTime = Time.time;
        }
        else if(Time.time - lastSeenPlayerTime <= persitanceDuration)
        {
            info.TargetPlayer = target;
            info.LastSeenPlayerLocation = info.TargetPlayer.transform.position;
        }
    }


    public GameObject CheckForPlayerInSight(float angle, float distance)
    {
        //if (target == null) return false;

        var pos = gameObject.transform.position;
        var playerPos = Camera.main.transform.position;

        var flatPos = new Vector3(pos.x, 0, pos.z);
        var flatPlayerPos = new Vector3(playerPos.x, 0, playerPos.z);

        if (Vector3.Distance(flatPos, flatPlayerPos) > distance) return null;

        var eyesCenter = eyes.GetComponent<SkinnedMeshRenderer>().bounds.center;
        Vector3 dir = (playerPos - eyesCenter);


        RaycastHit hit;
        if (Physics.Raycast(eyesCenter, dir, out hit, Mathf.Infinity, findPlayerMask))
        {
            Debug.DrawLine(eyesCenter, hit.point);
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == target && Vector3.Angle(dir, eyes.transform.forward) < angle * 0.5f)
            {
                return target;
            }
        }
        return null;
    }

    public GameObject CheckForBlockingDoor()
    {
        var doorObj = IsDoorInFront();
        if (doorObj != null & info.CurrentRoom != null & info.DestinationRoom != null)
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

    public float DistanceToTarget(GameObject target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }


    GameObject IsDoorInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, doorFindDistance, findDoorMask))
        {
            if (hit.transform.CompareTag("Door"))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
}
