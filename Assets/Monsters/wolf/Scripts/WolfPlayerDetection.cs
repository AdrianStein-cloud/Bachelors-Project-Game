using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfPlayerDetection : MonoBehaviour
{
    public float detectionRange;

    [SerializeField] LayerMask blocking;
    [SerializeField] GameObject[] visiblePoints;
    new Collider collider;

    WolfInfo info;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        info = GetComponent<WolfInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        info.canSeePlayer = CheckIfPlayerNearby(out info.TargetPlayer);

        if (info.canSeePlayer) info.seenByPlayer = PlayerCanSee();
    }

    bool CheckIfPlayerNearby(out GameObject player)
    {
        player = null;

        var tempPlayer = GameObject.FindGameObjectWithTag("Player");

        if (Vector3.Distance(transform.position, tempPlayer.transform.position) <= detectionRange)
        {
            player = tempPlayer;
            return true;
        }

        return false;
    }

    bool PlayerCanSee()
    {
        return IsVisible(Camera.main);
    }

    bool IsVisible(Camera camera)
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
}
