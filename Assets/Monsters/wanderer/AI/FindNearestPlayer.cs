using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Action("GameObject/FindNearestPlayer")]
[Help("Finds a game object by name")]
public class FindNearestPlayer : GOAction
{
    [InParam("tag")]
    [Help("Tag of the target game object")]
    public string tag;

    [OutParam("foundGameObject")]
    [Help("Found game object")]
    public GameObject foundGameObject;

    public override void OnStart()
    {
        var players = GameObject.FindGameObjectsWithTag(tag);

        if (players.Count() <= 0) return;

        var found = players[0];
        var currentDistance = Vector3.Distance(gameObject.transform.position, found.transform.position);
        foreach (var p in players)
        {
            var tempDistance = Vector3.Distance(gameObject.transform.position, p.transform.position);
            if (tempDistance < currentDistance)
            {
                currentDistance = tempDistance;
                found = p;
            }
        }
        foundGameObject = found;
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.COMPLETED;
    }
}
