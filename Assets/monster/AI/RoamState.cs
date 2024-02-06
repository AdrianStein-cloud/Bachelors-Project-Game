using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("Animation/RoamState")]
[Help("Roams")]
public class RoamState : GOAction
{
    private UnityEngine.AI.NavMeshAgent navAgent;

    ///<value>Input game object Parameter that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted.</value>
    [InParam("area")]
    [Help("game object that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted")]
    public GameObject area;

    [InParam("range")]
    public int range;

    /// <summary>Initialization Method of MoveToRandomPosition.</summary>
    /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
    /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
    public override void OnStart()
    {
        gameObject.GetComponent<WanderingBehaviour>().UpdateState(WanderingState.Wander);

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        navAgent.SetDestination(getRandomPosition());

        navAgent.isStopped = false;
    }
    /// <summary>Method of Update of MoveToRandomPosition </summary>
    /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
    /// and otherwise it will remain in operation.</remarks>
    public override TaskStatus OnUpdate()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            return TaskStatus.COMPLETED;
        return TaskStatus.RUNNING;
    }

    private Vector3 getRandomPosition()
    {
        return gameObject.transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
    }
    /// <summary>Abort method of MoveToRandomPosition </summary>
    /// <remarks>When the task is aborted, it stops the navAgentMesh.</remarks>
    public override void OnAbort()
    {
        navAgent.isStopped = true;
    }
}
