using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : Interactable
{
    [SerializeField] GameObject ReadyLamp;
    [SerializeField] GameObject NotReadyLamp;

    public bool DungeonIsAvailable { get; set; } = false;
    public Action EnterDungeon { get; set; }
    bool inFocus = false;

    private bool lightsOn = true;

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            if (DungeonIsAvailable)
            {
                Debug.Log("Enter dungeon");
                EnterDungeon();
            }
            else
            {
                Debug.LogWarning("Display dungeon not yet available");
            }
        }
        if(DungeonIsAvailable && !lightsOn)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            ReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            lightsOn = true;
        }
        else if(!DungeonIsAvailable && lightsOn)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            ReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            NotReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            lightsOn = false;
        }
    }
    public override void EnableInteractability()
    {
        inFocus = true;
    }

    public override void DisableInteractability()
    {
        inFocus = false;
    }

}
