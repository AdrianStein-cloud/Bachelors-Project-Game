using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStateController : StateController<WolfState>, IStunnable
{
    WolfInfo info;

    private void Awake()
    {
        info = GetComponent<WolfInfo>();
    }

    public void EndStun()
    {
        
    }

    public MonoBehaviour StartStun()
    {
        //Check if not dead
        if (!info.dead) SwitchState(WolfState.Dead);
        return this;
    }
}

public enum WolfState
{
    Bite,
    Flee,
    Roam,
    Dead
}
