using System;
using UnityEngine;
using UnityEngine.Events;

public class WandererSight : MonoBehaviour
{
    public GameObject eyes;
    public float detectionTime;

    [Header("Player Sight")]
    public LayerMask findPlayerMask;
    public float angle;
    public float distance;
    public float omniDirectionalVisionDistance;
    public float persitanceDuration = 0.4f;
    public float proximityRange = 15f;

    [Header("Door Sight")]
    public GameObject doorEyes;
    public float doorFindDistance;
    public LayerMask findDoorMask;
    public LayerMask lookThroughDoorMask;
    public float doorVisionReduce;

    public bool IsPlayerInSight => info.TargetPlayer != null;
    public bool IsThereBlockingDoor => door != null;

    private float firstTimeSeenPlayer;


    WandererInfo info;

    GameObject target;
    GameObject door;
    float lastSeenPlayerTime;
    bool buildingUpVision = false;

    GameObject hidingLocation;

    readonly Collider[] playerProximityResults = new Collider[5];
    readonly Collider[] hidingSpotColliderCastResults = new Collider[1];

    private void Awake()
    {
        info = GetComponent<WandererInfo>();

        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        door = CheckForBlockingDoor();
        if (door != null)
        {
            info.DoorToOpen = door;
        }


        GameObject targetPlayer = null;

        bool checkProximity = hidingLocation != null && Vector3.Distance(hidingLocation.transform.position, transform.position) < proximityRange;
        if (checkProximity) targetPlayer = CheckForPlayerInCloseProximity(proximityRange);
        if (targetPlayer == null) targetPlayer = CheckForPlayerInSight(360f, omniDirectionalVisionDistance);
        if (targetPlayer == null) targetPlayer = CheckForPlayerInSight(angle, distance);

        info.TargetPlayer = targetPlayer;

        if (info.TargetPlayer != null)
        {
            if ((buildingUpVision && Time.time >= firstTimeSeenPlayer + detectionTime))
            {
                info.LastSeenPlayerLocation = info.TargetPlayer.transform.position;
                lastSeenPlayerTime = Time.time;
            }
            else if (!buildingUpVision)
            {
                firstTimeSeenPlayer = Time.time;
                buildingUpVision = true;
                info.TargetPlayer = null;
            }
            else
            {
                info.TargetPlayer = null;
            }
        }
        else if (Time.time - lastSeenPlayerTime <= persitanceDuration)
        {
            info.TargetPlayer = target;
            info.LastSeenPlayerLocation = info.TargetPlayer.transform.position;
        }
        else if (Time.time > lastSeenPlayerTime + 3f)
        {
            buildingUpVision = false;
        }

        CheckIfPlayerWasSeenHiding();
    }


    public GameObject CheckForPlayerInSight(float angle, float distance)
    {
        //if (target == null) return false;

        var eyeOffset = new Vector3(0, 2f, 0);

        var pos = gameObject.transform.position;
        var playerPos = Camera.main.transform.position - eyeOffset;

        var flatPos = new Vector3(pos.x, 0, pos.z);
        var flatPlayerPos = new Vector3(playerPos.x, 0, playerPos.z);

        if (Vector3.Distance(flatPos, flatPlayerPos) > distance) return null;

        var eyesCenter = eyes.GetComponent<SkinnedMeshRenderer>().bounds.center - eyeOffset;
        Vector3 dir = (playerPos - eyesCenter);


        RaycastHit hit;
        if (Physics.Raycast(eyesCenter, dir, out hit, Mathf.Infinity, findPlayerMask))
        {
            Debug.DrawLine(eyesCenter, hit.point);
            //Debug.Log(hit.collider.gameObject.name);
            var flatDir = flatPlayerPos - flatPos;
            if (hit.collider.gameObject == target && Vector3.Angle(flatDir, transform.forward) < angle * 0.5f)
            {
                var hits = Physics.RaycastAll(eyesCenter, dir, hit.distance, lookThroughDoorMask);

                if (hit.distance > distance * Mathf.Pow(doorVisionReduce, hits.Length)) return null;

                return target;
            }
        }
        return null;
    }

    public GameObject CheckForPlayerInCloseProximity(float range)
    {
        int hitAmount = Physics.OverlapSphereNonAlloc(transform.position, range, playerProximityResults, LayerMask.GetMask("Player"));
        if (hitAmount > 0)
        {
            var playerObj = playerProximityResults[0].gameObject;
            return playerObj;
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
    }

    void CheckIfPlayerWasSeenHiding()
    {
        int hitAmount = Physics.OverlapSphereNonAlloc(info.LastSeenPlayerLocation, 1, hidingSpotColliderCastResults, LayerMask.GetMask("HidingSpot"));
        if (hitAmount == 0) return;
        var hidingSpot = hidingSpotColliderCastResults[0].GetComponent<HidingSpot>();
        info.LastSeenPlayerLocation = hidingSpot.FoundLocation.transform.position;
        hidingLocation = hidingSpot.FoundLocation;
    }

    public float DistanceToTarget(GameObject target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }


    GameObject IsDoorInFront()
    {
        RaycastHit hit;
        var raycastStartPoint = doorEyes.transform.position;
        Debug.DrawLine(raycastStartPoint, raycastStartPoint + doorEyes.transform.forward * doorFindDistance);
        if (Physics.Raycast(raycastStartPoint, doorEyes.transform.forward, out hit, doorFindDistance, findDoorMask))
        {
            if (hit.transform.CompareTag("Door"))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
}
