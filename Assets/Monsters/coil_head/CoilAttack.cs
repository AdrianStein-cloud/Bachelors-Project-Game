using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CoilAttack : StateProcess<CoilState>
{
    new Collider collider;
    CoilInfo info;
    NavMeshAgent agent;

    public LayerMask blocking;
    public GameObject[] visiblePoints;
    public AnimationClip[] poses;

    public float speed;

    Animation anim;

    Vector3 targetPos => info.Target.transform.position;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        info = GetComponent<CoilInfo>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        anim = GetComponentInChildren<Animation>();
    }

    private void OnEnable()
    {
        info.Target = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
        {
            //Debug.Log("Planes hit");
            bool isVisible = visiblePoints.FirstOrDefault(CanPlayerSeeGameobject);
            if (isVisible)
            {
                agent.isStopped = true;
                //Debug.Log("The collider is visible by the camera");
                return;

            }
        }
        if (agent.isStopped) DoRandomPose();
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
        agent.SetDestination(targetPos);
        agent.isStopped = false;
        //Debug.Log("The collider is not visible by the camera");
    }

    bool CanPlayerSeeGameobject(GameObject obj)
    {
        var pos = obj.transform.position;
        var dir = targetPos - pos;
        Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity, blocking);
        Debug.DrawLine(pos, pos + dir.normalized * hit.distance);
        return hit.collider.CompareTag("Player");
    }

    void DoRandomPose()
    {
        var clip = poses[Random.Range(0, poses.Length)];
        anim.Play(clip.name);
        Debug.Log("Playing anim: " + clip.name);
    }
}
