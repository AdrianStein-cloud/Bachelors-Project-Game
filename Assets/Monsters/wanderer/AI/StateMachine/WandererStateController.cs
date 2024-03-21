using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererStateController : StateController<WandererState>, IStunnable
{
    bool stunned = false;

    public MonoBehaviour StartStun()
    {
        if (!stunned)
        {
            stunned = true;
            InterruptWith(WandererState.Stunned);
        }
        return this;
    }
    public void EndStun()
    {
        if (currentState != WandererState.Stunned) Debug.LogError($"Expected state was Stunned, found {currentState}");
        stateInterruptsMap[WandererState.Stunned].Done();
        stunned = false;
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
