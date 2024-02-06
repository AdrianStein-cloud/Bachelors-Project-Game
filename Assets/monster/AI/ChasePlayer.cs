using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("Navigation/ChasePlayer")]
[Help("Chases the player until not visible")]
public class ChasePlayer : GOAction
{
    ///<value>Input target game object towards this game object will be moved Parameter.</value>
    [InParam("target")]
    [Help("Target game object towards this game object will be moved")]
    public GameObject target;

    private UnityEngine.AI.NavMeshAgent navAgent;

    private Transform targetTransform;

    /// <summary>Initialization Method of MoveToGameObject.</summary>
    /// <remarks>Check if GameObject object exists and NavMeshAgent, if there is no NavMeshAgent, the default one is added.</remarks>
    public override void OnStart()
    {
        if (target == null)
        {
            Debug.LogError("The movement target of this game object is null", gameObject);
            return;
        }
        targetTransform = target.transform;

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.SetDestination(targetTransform.position);

        navAgent.isStopped = false;
    }

    /// <summary>Method of Update of MoveToGameObject.</summary>
    /// <remarks>Verify the status of the task, if there is no objective fails, if it has traveled the road or is near the goal it is completed
    /// y, the task is running, if it is still moving to the target.</remarks>
    public override TaskStatus OnUpdate()
    {
        if (target == null)
            return TaskStatus.FAILED;
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            return TaskStatus.COMPLETED;
        else if (navAgent.destination != targetTransform.position)
            navAgent.SetDestination(targetTransform.position);
        return TaskStatus.RUNNING;
    }
}
