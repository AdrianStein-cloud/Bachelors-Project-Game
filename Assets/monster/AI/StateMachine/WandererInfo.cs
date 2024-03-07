using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererInfo : MonoBehaviour
{
    public GameObject DestinationRoom;
    public GameObject CurrentRoom;
    public GameObject DoorToOpen;
    public GameObject TargetPlayer;
    public Vector3 LastSeenPlayerLocation;
    public bool IsChasing;
}
