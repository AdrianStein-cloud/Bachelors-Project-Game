using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Framework;
using Pada1.BBCore.Framework.Internal;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Action("Print custom message")]
public class PrintNode : GOAction
{
    [InParam("PrintMessage")]
    public string message;

    public override void OnStart()
    {
        Debug.Log(message);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.COMPLETED;
    }

}
