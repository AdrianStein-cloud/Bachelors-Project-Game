using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererSearchRoom : StateProcess<WandererState>
{
    private void OnEnable()
    {
        Debug.Log("Serach Room");
        StateController.SwitchState(WandererState.Roam);
    }
}
