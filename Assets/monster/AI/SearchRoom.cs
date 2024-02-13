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
    private Room currentRoom;

    public override void OnStart()
    {
        wander = gameObject.GetComponent<WanderingBehaviour>();
        currentRoom = wander.currentRoom.GetComponent<Room>();
        if (currentRoom.isCorridor) return;

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        var roomScript = roomToSearch.GetComponent<Room>();

        var dest = RandomNavSphere(roomScript.centerObject.transform.position, wanderRadius, -1);
        navAgent.SetDestination(dest);
        navAgent.isStopped = false;

        Debug.DrawLine(gameObject.transform.position, dest);
    }

    public override TaskStatus OnUpdate()
    {
        if (currentRoom.isCorridor) return TaskStatus.FAILED;

        if (wander.state == WanderingState.Chase || (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance))
        {
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    // Generates a random point within a sphere
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = new Vector3(Random.Range(0.5f, 1f), origin.y, Random.Range(0.5f, 1f)) * distance;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
