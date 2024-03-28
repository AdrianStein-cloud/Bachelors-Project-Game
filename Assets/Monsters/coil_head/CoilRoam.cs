using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoilRoam : StateProcess<CoilState>
{
    public float speed;

    CoilInfo info;
    CoilMovement movement;

    List<GameObject> rooms = new List<GameObject>();

    Vector3 roamDestination;


    private void Awake()
    {
        info = GetComponent<CoilInfo>();
        movement = GetComponent<CoilMovement>();
    }

    private void OnEnable()
    {
        Roam(GetNextRoom());
    }

    private void FixedUpdate()
    {
        if (info.Target != null)
        {
            StateController.SwitchState(CoilState.Attack);
            return;
        }

        bool destinationReached = Vector3.Distance(roamDestination, transform.position) <= 10f;
        if (destinationReached || info.ThereIsBockingDoor)
        {
            var room = GetNextRoom();
            Roam(room);
        }
    }

    void Roam(GameObject room)
    {
        var destRoom = room.GetComponent<Room>();
        roamDestination = destRoom.centerObject.transform.position;
        movement.SetTargetPosition(roamDestination, speed);
    }

    //Get a random room, but remove from list after. Ensures entire dungeon will be roamed
    private GameObject GetNextRoom()
    {
        //When room count is 0 (has roamed entire map), resets the list and begins roaming again
        if (rooms.Count <= 0) rooms = new List<GameObject>(FindObjectOfType<DungeonGenerator>().spawnedRooms);
        rooms = rooms
            .OrderBy(r => Vector3.Distance(new Vector3(0, gameObject.transform.position.y, 0), new Vector3(0, r.transform.position.y, 0)))
            .OrderBy(r => Vector3.Distance(new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z), new Vector3(r.transform.position.x, 0, r.transform.position.z)))
            .ToList();

        var room = rooms[0];
        rooms.Remove(room);
        return room;
    }
}
