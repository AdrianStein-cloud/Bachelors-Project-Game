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

        var room = wander.wanderingTo;
        if (wander.hasReachedRoom)
        {
            room = GetRandomPosition();
            wander.hasReachedRoom = false;
        }

        var roomscript = room.GetComponent<Room>();

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.SetDestination(roomscript.centerObject.transform.position);
        navAgent.isStopped = false;

        wander.roomsCopy = rooms;
        wander.wanderingTo = room;
        outRoom = room;
    }

    public override TaskStatus OnUpdate()
    {
        if (wander.state == WanderingState.Chase 
            || (!navAgent.pathPending && navAgent.remainingDistance <= stopRange))
        {
            wander.hasReachedRoom = true;
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    private GameObject GetRandomPosition()
    {
        //When room count is 0 (has roamed entire map), resets the list and begins roaming again
        if (rooms.Count <= 0) rooms = new List<GameObject>(GameObject.FindObjectOfType<DungeonGenerator>().spawnedRooms);

        rooms = rooms.OrderBy(r => Vector3.Distance(new Vector3(0, gameObject.transform.position.y, 0), 
            new Vector3(0, r.transform.position.y, 0)))
            .OrderBy(r => Vector3.Distance(new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z), 
            new Vector3(r.transform.position.x, 0, r.transform.position.z))).ToList();

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
