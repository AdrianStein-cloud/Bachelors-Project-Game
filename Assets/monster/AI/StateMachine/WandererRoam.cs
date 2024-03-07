using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WandererRoam : StateProcess<WandererState>
{
    public float speed;

    WandererMovement movement;
    WandererSight sight;
    WandererInfo info;

    Animator anim;

    List<GameObject> rooms = new List<GameObject>();
    public Room destRoom;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        sight = GetComponent<WandererSight>();
        info = GetComponent<WandererInfo>();

        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Roam();
    }

    private void OnDisable()
    {
        anim.SetBool("Wander", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StateController.SwitchState(WandererState.LookingForPlayer);
            return;
        }
        if (sight.IsThereBlockingDoor)
        {
            StateController.InterruptWith(WandererState.OpenDoor);
            return;
        }

        if (sight.IsPlayerInSight)
        {
            destRoom = null;
            StateController.SwitchState(WandererState.FoundPlayer);
            return;
        }

    }

    void Roam()
    {
        anim.SetBool("Wander", true);

        if(destRoom == null)
        {
            destRoom = GetNextRoom().GetComponent<Room>();
        }

        //Debug.Log("Wandering to room");
        movement.MoveTo(destRoom.centerObject.transform.position, speed, DestinationReached);
    }

    void DestinationReached()
    {
        //Debug.Log("Destination Reached");

        //Only transition if roaming didn't get interrupted (currently it cant get interrupted, but just in case)
        if (enabled)
        {
            destRoom = null;
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
