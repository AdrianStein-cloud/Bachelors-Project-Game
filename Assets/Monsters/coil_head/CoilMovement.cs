using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CoilMovement : MonoBehaviour
{

    [SerializeField] LayerMask blocking;
    [SerializeField] GameObject[] visiblePoints;
    [SerializeField] AnimationClip[] poses;

    new Collider collider;
    CoilInfo info;
    Animation anim;
    NavMeshAgent agent;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        info = GetComponent<CoilInfo>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        anim = GetComponentInChildren<Animation>();
    }

    private void FixedUpdate()
    {
        bool isVisible = IsCoilVisisble(Camera.main);

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
        }
    }

    public void SetTargetPosition(Vector3 position, float speed)
    {
        if (agent.path.status == NavMeshPathStatus.PathComplete)
        {
            agent.speed = speed;
            agent.SetDestination(position);
        }
        else
        {
            Debug.LogWarning("NavMesh path not complete");
        }
    }

    bool IsCoilVisisble(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
        {
            //Nessecary as player camera is always a bit offset from player controller due to smoothing
            //Needs changes if it so to work with other cameras, fx security camera gadget
            Vector3 cameraPosition = camera.transform.root.GetComponentInChildren<CharacterController>().transform.position;

            bool isVisible = visiblePoints.FirstOrDefault((g) => CanPlayerSeeGameobject(g, cameraPosition));
            if (isVisible)
            {
                return true;
            }
        }
        return false;
    }

    bool CanPlayerSeeGameobject(GameObject obj, Vector3 cameraPosition)
    {
        var pos = obj.transform.position;
        var dir = cameraPosition - pos;
        Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity, blocking);
        Debug.DrawLine(pos, pos + dir.normalized * hit.distance);
        return hit.collider.CompareTag("Player");
    }

    void DoRandomPose()
    {
        var clip = poses[Random.Range(0, poses.Length)];
        anim.Play(clip.name);
        //Debug.Log("Playing anim: " + clip.name);
    }
}
