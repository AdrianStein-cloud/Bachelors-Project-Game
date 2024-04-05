using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        Debug.Log("RUN");

        rooms = new List<GameObject>(GameObject.FindObjectOfType<DungeonGenerator>().spawnedRooms).OrderBy(g => Vector3.Distance(transform.position, g.transform.position)).ToList();

        var farRooms = rooms.Where(g => Vector3.Distance(transform.position, g.transform.position) > minimumRange).ToList();

        anim.SetBool("Run", true);
        var roomCenter = farRooms.Count <= 0 ? rooms[rooms.Count - 1].GetComponent<Room>().centerObject : farRooms[0].GetComponent<Room>().centerObject;
        //Debug.Log(roomCenter.transform.position);
        movement.MoveTo(roomCenter.transform.position, speed, () => controller.SwitchState(WolfState.Roam));
    }

    private void OnDisable()
    {
        movement.Stop();
        anim.SetBool("Run", false);
    }
}
