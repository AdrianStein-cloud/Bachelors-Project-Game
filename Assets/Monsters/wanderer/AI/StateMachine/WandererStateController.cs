using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererStateController : StateController<WandererState>, IStunnable, IDistractable
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

    public void Distract(Vector3 position)
    {
        GetComponent<WandererInfo>().DecoyPosition = position;
        SwitchState(WandererState.Distracted);
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
    Stunned,
    Distracted
}
