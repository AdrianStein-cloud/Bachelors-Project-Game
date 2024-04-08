using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfFlee : StateProcess<WolfState>
{
    [Header("Flee")]
    public float minimumRange;
    public float speed;

    Animator anim;
    WolfInfo info;
    WolfMovement movement;
    WolfStateController controller;
    WolfSounds sounds;

    List<GameObject> rooms;

    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        sounds = GetComponent<WolfSounds>();
        info = GetComponent<WolfInfo>();
        movement = GetComponent<WolfMovement>();
        controller = GetComponent<WolfStateController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        player = info.TargetPlayer;
        anim.SetBool("Run", true);
        Debug.Log("RUN");

        /*rooms = new List<GameObject>(UnitySingleton<Dungeon>.Instance.Rooms.Select(r => r.gameObject)).OrderBy(g => Vector3.Distance(transform.position, g.transform.position)).ToList();

        var farRooms = rooms.Where(g => Vector3.Distance(transform.position, g.transform.position) > minimumRange).ToList();

        var roomCenter = farRooms.Count <= 0 ? rooms[rooms.Count - 1].GetComponent<Room>().centerObject : farRooms[0].GetComponent<Room>().centerObject;
        movement.MoveTo(roomCenter.transform.position, speed, () => controller.SwitchState(WolfState.Roam));*/
        Flee();
    }

    void Flee()
    {
        var playerPos = player.transform.position;
        var distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        if (distanceToPlayer > minimumRange) {
            controller.SwitchState(WolfState.Roam);
            return;
        }
        var currentRoom = UnitySingleton<Dungeon>.Instance.GetCurrentRoom(transform.position);
        if(currentRoom == null)
        {
            Debug.LogWarning("Wolf died of confusion"); //Couldn't get current room (wolf was probably bugged or in a connection room)
            controller.SwitchState(WolfState.Dead);
            return;
        }
        var diff = playerPos - transform.position;
        var escapeDoor = currentRoom.AdjecentRooms.OrderByDescending(p => Vector3.Distance(p.Key.transform.position, playerPos)).First();
        var nextFleePosition = escapeDoor.Key.transform.position;
        if (distanceToPlayer > Vector3.Distance(nextFleePosition, playerPos))
        {
            if(info.seenByPlayer)
            {
                var farFleePosition = GetFarAwayRoom();
                movement.MoveTo(farFleePosition, speed * 1.5f, Flee);
            }
            else
            {
                controller.SwitchState(WolfState.Roam);
                return;
            }
        }
        else
        {
            movement.MoveTo(escapeDoor.Value.centerObject.transform.position, speed, Flee);
        }
    }

    Vector3 GetFarAwayRoom()
    {
        var rooms = UnitySingleton<Dungeon>.Instance.Rooms.OrderBy(g => Vector3.Distance(transform.position, g.transform.position)).ToList();

        var farRooms = rooms.Where(g => Vector3.Distance(transform.position, g.transform.position) > minimumRange).ToList();

        var roomCenter = farRooms.Count <= 0 ? rooms[rooms.Count - 1].GetComponent<Room>().centerObject : farRooms[0].GetComponent<Room>().centerObject;
        return roomCenter.transform.position;
    }

    private void OnDisable()
    {
        movement.Stop();
        anim.SetBool("Run", false);
    }
}
