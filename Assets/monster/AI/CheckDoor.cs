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

    [InParam("layerMask")]
    public LayerMask layerMask;

    public override bool Check()
    {
        var colliders = Physics.OverlapSphere(gameObject.transform.position, closeDistance, layerMask);
        foreach (var col in colliders)
        {
            if (col.CompareTag(doortag))
            {
                var door = col.transform.parent.gameObject;
                var doorScript = door.GetComponent<ToggleDoor>();
                if (doorScript.isLocked) Debug.Log("Needs to implement if door is locked");
                gameObject.GetComponent<WanderingBehaviour>().doorToOpen = door;
                return !doorScript.open;
            }
        }
        return false;
    }
}
