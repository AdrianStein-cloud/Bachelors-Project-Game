using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[Action("Roam")]
[Help("Roams the map")]
public class RoamState : GOAction
{
    private UnityEngine.AI.NavMeshAgent navAgent;

    [InParam("stopRange")]
    public int stopRange;

    [OutParam("RoomToSearch")]
    public GameObject outRoom;

    private List<GameObject> rooms;
    private WanderingBehaviour wander;

    public override void OnStart()
    {
        wander = gameObject.GetComponent<WanderingBehaviour>();

        if (!wander.once)
        {
            rooms = new List<GameObject>(GameObject.FindObjectOfType<DungeonGenerator>().spawnedRooms);
            wander.once = true;
        }
        else rooms = wander.roomsCopy;

        var room = GetRandomPosition();

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.SetDestination(room.transform.position);
        navAgent.isStopped = false;

        wander.roomsCopy = rooms;
        wander.wanderingTo = room;
        outRoom = room;
    }

    public override TaskStatus OnUpdate()
    {
        if (wander.state == WanderingState.Chase || (!navAgent.pathPending && navAgent.remainingDistance <= stopRange))
        {
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    private GameObject GetRandomPosition()
    {
        if (rooms.Count <= 0) Debug.Log("Rooms list empty. Fix: Restart process aka restart list");
        rooms = rooms.OrderBy(r => Vector3.Distance(gameObject.transform.position, r.transform.position)).ToList();

        var room = rooms[0];
        rooms.Remove(room);
        //rooms.Add(room);
        wander.roomsCopy = rooms;
        return room;
    }

    public override void OnAbort()
    {
        navAgent.isStopped = true;
    }
}
