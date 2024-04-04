using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Quaternion direction;
    private bool doorConnected = false;
    public bool debugHighlight = false;

    [Header("Bounding Box")]
    [SerializeField] float bounding_x = 15;
    [SerializeField] float bounding_y = 23;
    [SerializeField] float bounding_z = 2;
    [Header("Setup")]
    [SerializeField] public bool isEntrance = false;
    [SerializeField] GameObject doorWallBlocker;
    [SerializeField] GameObject doorRemoveIfNoRoom;

    private void Awake()
    {
        direction = transform.rotation;
        if (doorWallBlocker != null)
        {
            doorWallBlocker.SetActive(true);
            if(doorRemoveIfNoRoom != null)
            {
                doorRemoveIfNoRoom.SetActive(false);
            }
        }
    }

    public void SetDoorConnected(bool connected)
    {
        doorConnected = connected;
        if(doorWallBlocker != null)
        {
            if (connected) Destroy(doorWallBlocker);
            if (doorRemoveIfNoRoom != null)
            {
                doorRemoveIfNoRoom.SetActive(connected);
            }
        }
    }

    public bool GetDoorConnected()
    {
        return doorConnected;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        if (debugHighlight)
        {
            Gizmos.color = Color.blue;
        }
        else if (doorConnected)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(new Vector3(0, bounding_y / 2, 0), new Vector3(bounding_x, bounding_y, bounding_z));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(bounding_x/2, 0, 0), new Vector3(-bounding_x/2, 0, 0));
        
        if (isEntrance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(5, 5, 5));
            Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, -10));
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(new Vector3(0, 0, 0), 2.5f);
            Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 20));
        }
    }
}
