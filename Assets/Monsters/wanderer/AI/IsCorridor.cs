using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Condition("IsCorridor")]
public class IsCorridor : GOCondition
{
    public override bool Check()
    {
        var room = gameObject.GetComponent<WanderingBehaviour>().currentRoom;
        if (room == null) return false;

        var iscorridor = room.GetComponent<Room>().isCorridor;
        return iscorridor;
    }
}
