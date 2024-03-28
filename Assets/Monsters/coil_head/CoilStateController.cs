using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilStateController : StateController<CoilState>, IStunnable
{    
    public MonoBehaviour StartStun()
    {
        Debug.LogWarning("Coil start stun isn't implemented");
        return this;
    }

    public void EndStun()
    {
        Debug.LogWarning("Coil end stun isn't implemented");
    }

}

public enum CoilState
{
    Attack,
    Roam
}
