using BBUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererSearchRoom : StateProcess<WandererState>
{
    WandererInfo info;
    WandererMovement movement;

    public float speed;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        info = GetComponent<WandererInfo>();
    }

    private void OnEnable()
    {
        Debug.Log("Serach Room");

        var roomScript = info.CurrentRoom.GetComponent<Room>();
        var roamPoint = roomScript.roamPositions[Random.Range(0, roomScript.roamPositions.Count)];
        movement.MoveTo(roamPoint.transform.position, speed, DestinationReached);
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
