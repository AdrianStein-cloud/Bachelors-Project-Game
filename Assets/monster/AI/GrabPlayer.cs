using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("GrabPlayer")]
public class GrabPlayer : GOAction
{
    [InParam("target")]
    public GameObject target;

    [InParam("dirObject")]
    public GameObject dirObject;

    public override void OnStart()
    {
        InputManager.Player.Disable();

        //player camera look at enemy
        var dirobjectcenter = dirObject.GetComponent<SkinnedMeshRenderer>().bounds.center;
        Camera.main.transform.parent.LookAt(dirobjectcenter);
    }

}
