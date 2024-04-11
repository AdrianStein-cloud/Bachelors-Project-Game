using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CoilMovement : MonoBehaviour
{

    [SerializeField] GameObject doorEyes;
    [SerializeField] float doorFindDistance = 10f;
    [SerializeField] LayerMask blocking;
    [SerializeField] GameObject[] visiblePoints;
    [SerializeField] AnimationClip[] poses;

    new Collider collider;
    CoilInfo info;
    Animation anim;
    NavMeshAgent agent;
    EnemyVisionInfo visionInfo;

    CameraManager cameraManager => UnitySingleton<CameraManager>.Instance;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        info = GetComponent<CoilInfo>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        anim = GetComponentInChildren<Animation>();
        visionInfo = GetComponent<EnemyVisionInfo>();
    }

    private void FixedUpdate()
    {
        var camPositions = cameraManager.cameraPositions;
        bool isVisible = UnitySingleton<CameraManager>.Instance.activeCameras.FirstOrDefault(cam => IsCoilVisisble(cam, camPositions[cam])) != null;
        visionInfo.CanSeePlayer = isVisible;

        //bool isVisible = IsCoilVisisble(Camera.main);

        if (isVisible)
        {
            agent.isStopped = true;
            info.IsVisible = true;
        }
        else
        {
            if(info.IsVisible) DoRandomPose(); //Update pose if coil was visible last frame


            agent.isStopped = false;
            info.IsVisible = false;
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);

            info.ThereIsBockingDoor = IsThereBlockingDoor();
            if (info.ThereIsBockingDoor)
            {
                agent.isStopped = true;
            }
        }
    }

    public void SetTargetPosition(Vector3 position, float speed)
    {
        agent.speed = speed;
        agent.SetDestination(position);
    }

    bool IsCoilVisisble(Camera camera, GameObject cameraCollider)
    {
        if (camera == null) return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
        {
            //Nessecary as player camera is always a bit offset from player controller due to smoothing
            //Needs changes if it so to work with other cameras, fx security camera gadget
            //Vector3 cameraPosition = camera.transform.root.GetComponentInChildren<CharacterController>().transform.position;

            bool isVisible = visiblePoints.FirstOrDefault((g) => CanPlayerSeeGameobject(g, cameraCollider));
            if (isVisible)
            {
                return true;
            }
        }
        return false;
    }

    bool CanPlayerSeeGameobject(GameObject obj, GameObject cameraSrc)
    {
        var pos = obj.transform.position;
        var dir = cameraSrc.transform.position - pos;
        Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity, blocking + LayerMask.GetMask(LayerMask.LayerToName(cameraSrc.layer)));
        Debug.DrawLine(pos, pos + dir.normalized * hit.distance);
        return hit.collider.gameObject == cameraSrc;
    }

    bool IsThereBlockingDoor()
    {
        var doorObj = IsDoorInFront();
        if (doorObj != null)
        {
            var door = doorObj.GetComponent<ToggleDoor>();
            if (!door.open) return true;
        }
        return false;
    }

    GameObject IsDoorInFront()
    {
        var direction = PathingDirection();
        RaycastHit hit;
        var raycastStartPoint = doorEyes.transform.position;
        Debug.DrawLine(raycastStartPoint, raycastStartPoint + direction * doorFindDistance);
        if (Physics.Raycast(raycastStartPoint, direction, out hit, doorFindDistance, LayerMask.GetMask("Door")))
        {
            if (hit.transform.CompareTag("Door"))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    Vector3 PathingDirection()
    {
        if (agent.hasPath && agent.path.corners.Length > 1)
        {
            Vector3 direction = (agent.path.corners[1] - agent.transform.position).normalized;
            return direction;
        }
        else
        {
            //Debug.LogWarning("Defaulting to transform forward instead of agent path");
            return transform.forward;
        }
    }

    void DoRandomPose()
    {
        var clip = poses[Random.Range(0, poses.Length)];
        anim.Play(clip.name);
        //Debug.Log("Playing anim: " + clip.name);
    }
}
