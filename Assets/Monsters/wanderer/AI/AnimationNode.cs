using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction
using BBUnity.Actions;
using UnityEditor;

[Action("MyActions/DoAnimation")]
[Help("Using the animator, can set bools, triggers etc.")]

public class AnimationNode : GOAction
{
    [InParam("ParamType")]
    public AnimationParameterType paramType;

    [InParam("ParamName")]
    public string paramName;

    [InParam("FloatValue")]
    public float floatValue;

    [InParam("IntValue")]
    public int intValue;

    [InParam("BoolValue")]
    public bool boolValue;


    public override void OnStart()
    {
        Animator anim = gameObject.GetComponent<Animator>();

        switch (paramType)
        {
            case AnimationParameterType.Boolean:
                anim.SetBool(paramName, true);
                break;
            case AnimationParameterType.Trigger:
                anim.SetTrigger(paramName);
                break;
            case AnimationParameterType.Int:
                anim.SetBool(paramName, true);
                break;
            case AnimationParameterType.Float:
                break;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.COMPLETED;
    }
}

public enum AnimationParameterType
{
    Trigger,
    Boolean,
    Int,
    Float
}

