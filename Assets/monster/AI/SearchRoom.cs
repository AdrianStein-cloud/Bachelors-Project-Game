using BBUnity.Actions;
using JetBrains.Annotations;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Action("Search")]
[Help("Searches room")]
public class SearchRoom : GOAction
{
    [InParam("WanderRadius")]
    public float wanderRadius;

    [InParam("CurrentRoom")]
    public GameObject roomToSearch;

    private NavMeshAgent navAgent;
    private WanderingBehaviour wander;

    public override void OnStart()
    {
        wander = gameObject.GetComponent<WanderingBehaviour>();

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        var roomScript = roomToSearch.GetComponent<Room>();

        var dest = RandomNavSphere(roomScript.centerObject.transform.position, wanderRadius, -1);
        navAgent.SetDestination(dest);
        navAgent.isStopped = false;

        Debug.DrawLine(gameObject.transform.position, dest);
    }

    public override TaskStatus OnUpdate()
    {
        if (wander.state == WanderingState.Chase || (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance))
        {
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    // Generates a random point within a sphere
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
