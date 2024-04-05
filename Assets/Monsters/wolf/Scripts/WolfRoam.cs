using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfRoam : StateProcess<WolfState>
{
    public float speed;

    WolfMovement movement;
    WolfInfo info;

    Animator anim;

    List<GameObject> rooms = new List<GameObject>();
    public Room destRoom;

    private void Awake()
    {
        movement = GetComponent<WolfMovement>();
        info = GetComponent<WolfInfo>();

        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        Debug.Log("Roam");
        Roam();
    }

    private void OnDisable()
    {
        movement.Stop();
        anim.SetBool("Walk", false);
    }

    private void Update()
    {
        if (info.canSeePlayer)
        {
            destRoom = null;
            StateController.SwitchState(WolfState.Bite);
            return;
        }

    }

    void Roam()
    {
        anim.SetBool("Walk", true);

        if (destRoom == null)
        {
            destRoom = GetNextRoom().GetComponent<Room>();
        }

        //Debug.Log("Wandering to room");
        movement.MoveTo(destRoom.centerObject.transform.position, speed, DestinationReached);
        //movement.MoveTo(GameObject.FindGameObjectWithTag("Player").transform.position, speed, () => StateController.SwitchState(WandererState.Roam));
    }

    void DestinationReached()
    {
        if (enabled)
        {
            StateController.SwitchState(WolfState.Roam);
            destRoom = null;
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
