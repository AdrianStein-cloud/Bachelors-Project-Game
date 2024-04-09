using BBUnity.Actions;
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

    Collider[] hidingSpotColliderCastResults = new Collider[10];

    GameObject tempPlayer;

    WolfInfo info;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        info = GetComponent<WolfInfo>();
        tempPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        info.canSeePlayer = CheckIfPlayerNearby(out info.TargetPlayer);

        if (info.canSeePlayer) info.seenByPlayer = PlayerCanSee();
    }

    bool CheckIfPlayerNearby(out GameObject player)
    {
        player = null;
        Vector3 playerPos = tempPlayer.transform.position;

        if (Vector3.Distance(transform.position, playerPos) <= detectionRange && !IsPlayerHiding(playerPos))
        {
            player = tempPlayer;
            return true;
        }

        return false;
    }

    bool IsPlayerHiding(Vector3 playerPosition)
    {
        int hitAmount = Physics.OverlapSphereNonAlloc(playerPosition, 1, hidingSpotColliderCastResults, LayerMask.GetMask("HidingSpot"));
        return hitAmount > 0;
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
