using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("Action/OpenDoor")]
public class OpenDoor : GOAction
{

    public override void OnStart()
    {
        gameObject.GetComponent<WanderingBehaviour>().doorToOpen.GetComponent<ToggleDoor>().Interact();
        gameObject.GetComponent<WanderingBehaviour>().doorToOpen = null;
    }
}
