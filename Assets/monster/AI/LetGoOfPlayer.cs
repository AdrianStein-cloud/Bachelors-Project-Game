using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("LetGoOfPlayer")]
public class LetGoOfPlayer : GOAction
{
    [InParam("target")]
    public GameObject target;

    public override void OnStart()
    {
        InputManager.Player.Enable();

        //player camera look at enemy
    }

    public override TaskStatus OnUpdate()
    {
        return base.OnUpdate();
    }
}
