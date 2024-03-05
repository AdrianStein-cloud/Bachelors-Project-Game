using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WandererRoam : StateProcess<WandererState>
{
    public float roamSpeed;

    WandererMovement movement;
    WandererInfo info;
    Animator anim;

    List<GameObject> rooms = new List<GameObject>();
    public Room room;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        anim = GetComponent<Animator>();
        info = GetComponent<WandererInfo>();
    }

    private void OnEnable()
    {
        Roam();
    }

    private void Update()
    {
        var door = movement.CheckForBlockingDoor();
        if (door != null)
        {
            info.DoorToOpen = door;
            StateController.InterruptWith(WandererState.OpenDoor);
        }
    }

    void Roam()
    {
        anim.SetBool("Wander", true);

        if(room == null)
        {
            room = GetNextRoom().GetComponent<Room>();
        }

        Debug.Log("Wandering to room");
        movement.MoveTo(room.centerObject.transform.position, roamSpeed, DestinationReached);
    }

    void DestinationReached()
    {
        Debug.Log("Destination Reached");

        //Only transition if roaming didn't get interrupted (currently it cant get interrupted, but just in case)
        if (enabled)
        {
            room = null;
            StateController.SwitchState(WandererState.SearchRoom);
        }
    }

    //Get a random room, but remove from list after. Ensures entire dungeon will be roamed
    private GameObject GetNextRoom()
    {
        //When room count is 0 (has roamed entire map), resets the list and begins roaming again
        if (rooms.Count <= 0) rooms = new List<GameObject>(GameObject.FindObjectOfType<DungeonGenerator>().spawnedRooms);
        rooms = rooms
            .OrderBy(r => Vector3.Distance(new Vector3(0, gameObject.transform.position.y, 0), new Vector3(0, r.transform.position.y, 0)))
            .OrderBy(r => Vector3.Distance(new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z), new Vector3(r.transform.position.x, 0, r.transform.position.z)))
            .ToList();

        var room = rooms[0];
        rooms.Remove(room);
        return room;
    }
}
