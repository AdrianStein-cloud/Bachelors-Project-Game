using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[Condition("Perception/IsDoorOpen")]
[Help("Checks whether a door infront is open or not. True is not open")]
public class CheckDoor : GOCondition
{
    [InParam("DoorTag")]
    public string doortag;

    [InParam("closeDistance")]
    public float closeDistance;

    [InParam("dirObject")]
    public GameObject dirObject;

    [InParam("layerMask")]
    public LayerMask layerMask;

    private WanderingBehaviour wander;

    public override bool Check()
    {
        RaycastHit hit;
        if (Physics.Raycast(dirObject.transform.position, dirObject.transform.forward, out hit, closeDistance, layerMask))
        {
            if (hit.transform.CompareTag(doortag))
            {
                wander = gameObject.GetComponent<WanderingBehaviour>();

                var doorscript = hit.transform.GetComponent<ToggleDoor>();

                if (doorscript.open) return false;

                wander.doorToOpen = hit.transform.gameObject;
                return true;
            }
        }
        return false;
    }
}
