using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererStateController : StateController<WandererState>
{
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
    FoundPlayer
}
