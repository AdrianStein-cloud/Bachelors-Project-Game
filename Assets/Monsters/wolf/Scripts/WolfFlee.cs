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

    GameObject player;


    void Awake()
    {
        info = GetComponent<WolfInfo>();
        movement = GetComponent<WolfMovement>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        player = info.TargetPlayer;
        anim.SetBool("Run", true);

        if (info.wasCornered | player == null)
        {
            info.wasCornered = false;
            FleeFarAway();
        }
        else
        {
            Flee();
        }
        
    }

    void Flee()
    {
        var playerPos = player.transform.position;
        var distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        if (distanceToPlayer > minimumRange) {
            StateController.SwitchState(WolfState.Roam);
            return;
        }
        var currentRoom = UnitySingleton<Dungeon>.Instance.GetCurrentRoom(transform.position);
        if(currentRoom == null)
        {
            FleeFarAway();
            return;
        }
        var escapeDoor = currentRoom.AdjecentRooms.OrderByDescending(p => Vector3.Distance(p.Key.transform.position, playerPos)).First();
        var nextFleePosition = escapeDoor.Key.transform.position;
        if (distanceToPlayer > Vector3.Distance(nextFleePosition, playerPos)) //If wolf can no longer run away, but only towards player
        {
            StateController.SwitchState(WolfState.Cornered);
        }
        else
        {
            movement.MoveTo(escapeDoor.Value.centerObject.transform.position, speed, Flee);
        }
    }

    void FleeFarAway()
    {
        var farAwayPos = GetFarAwayRoom();
        movement.MoveTo(farAwayPos, speed, () => StateController.SwitchState(WolfState.Roam));
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
