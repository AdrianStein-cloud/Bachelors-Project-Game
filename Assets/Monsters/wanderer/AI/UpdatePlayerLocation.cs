using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("UpdatePlayerLocation")]
public class UpdatePlayerLocation : GOAction
{
    [InParam("target")]
    public GameObject target;

    public override void OnStart()
    {
        var wander = gameObject.GetComponent<WanderingBehaviour>();
        wander.lastLocation = target.transform.position;
    }
}
