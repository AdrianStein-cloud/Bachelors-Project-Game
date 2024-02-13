using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("Action/OpenDoor")]
public class OpenDoor : GOAction
{
    private WanderingBehaviour wander;

    public override void OnStart()
    {
        wander = gameObject.GetComponent<WanderingBehaviour>();
        var door = wander.doorToOpen?.GetComponent<ToggleDoor>();
        door?.Interact(gameObject);
        wander.doorToOpen = null;
    }
}
