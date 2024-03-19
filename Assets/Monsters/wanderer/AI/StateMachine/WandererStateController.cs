using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererStateController : StateController<WandererState>, IStunnable
{
    public MonoBehaviour StartStun()
    {
        InterruptWith(WandererState.Stunned);
        return this;
    }
    public void EndStun()
    {
        if (currentState != WandererState.Stunned) Debug.LogError($"Expected state was Stunned, found {currentState}");
        stateInterruptsMap[WandererState.Stunned].Done();
    }

}

public enum WandererState
{
    Roam,
    SearchRoom,
    LookAround,
    Chase,
    Attack,
    OpenDoor,
    LookingForPlayer,
    FoundPlayer,
    Stunned
}
