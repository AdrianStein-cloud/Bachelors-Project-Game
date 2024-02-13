using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction
using BBUnity.Actions;
using UnityEditor;
using UnityEngine.AI;

[Action("MyActions/UpdateWanderingState")]
[Help("Updates the state of a wanderer")]

public class UpdateWanderingState : GOAction
{
    [InParam("New State")]
    [Help("State to transition to")]
    public WanderingState newState;

    [InParam("Blend Value")]
    [Help("Speed value for animation blend tree")]
    public float SpeedValue;

    [InParam("Movespeed value")]
    public float movespeed;

    public override void OnStart()
    {
        gameObject.GetComponent<NavMeshAgent>().speed = movespeed;
        gameObject.GetComponent<Animator>().SetFloat("Blend", SpeedValue/100);
        gameObject.GetComponent<Animator>().SetBool("Attack", newState == WanderingState.Attack);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.COMPLETED;
    }
}   

