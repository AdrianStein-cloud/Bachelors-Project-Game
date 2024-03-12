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
    bool iscorridor;

    public override void OnStart()
    {
        wander = gameObject.GetComponent<WanderingBehaviour>();
        currentRoom = wander.currentRoom.GetComponent<Room>();
        if (currentRoom.isCorridor)
        {
            iscorridor = true;
            return;
        }

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        var roomScript = currentRoom.GetComponent<Room>();

        var dest = RandomNavSphere(roomScript.centerObject.transform.position, roomScript.bounding_x, roomScript.bounding_z);
        navAgent.SetDestination(dest);
        navAgent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (wander.state == WanderingState.Chase || iscorridor
            || (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance))
        {
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    // Generates a random point within a sphere
    Vector3 RandomNavSphere(Vector3 origin, float distancex, float distancez)
    {
        Vector3 randomDirection = new Vector3(Random.Range(0.5f, 0.9f) * distancex / 2, origin.y, Random.Range(0.5f, 0.9f) * distancez / 2);

        return randomDirection += origin;
    }
}
