using BBUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererSearchRoom : StateProcess<WandererState>
{
    WandererInfo info;
    WandererMovement movement;
    Animator anim;

    public float speed;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetBool("Wander", true);

        var roomScript = info.CurrentRoom.GetComponent<Room>();
        if (roomScript.roamPositions.Count > 0)
        {
            var roamPoint = roomScript.roamPositions[Random.Range(0, roomScript.roamPositions.Count)];
            movement.MoveTo(roamPoint.transform.position, speed, DestinationReached);
        }
        else
        {
            Debug.Log("No roam positions found. Shouldnt happen i think");
            DestinationReached();
        }
    }

    private void OnDisable()
    {
        movement.Stop();
        anim.SetBool("Wander", false);
    }

    void DestinationReached()
    {
        //Debug.Log("Destination Reached");

        //Only transition if roaming didn't get interrupted (currently it cant get interrupted, but just in case)
        if (enabled)
        {
            StateController.SwitchState(WandererState.Roam);
        }
    }
}
