using UnityEngine;

public class WandererSight : MonoBehaviour
{
    public GameObject eyes;

    [Header("Player Sight")]
    public LayerMask findPlayerMask;
    public float angle;
    public float distance;

    [Header("Door Sight")]
    public float doorFindDistance;
    public LayerMask findDoorMask;


    WandererInfo info;

    GameObject target;

    private void Awake()
    {
        info = GetComponent<WandererInfo>();

        target = GameObject.FindGameObjectWithTag("Player");
    }


    public GameObject CheckForPlayerInSight()
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
