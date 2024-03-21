using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : Interactable
{
    public Func<bool> LeaveDungeon { get; set; }
    public bool CanLeaveDungeon { get; set; } = false;
    bool inFocus = false;

    [SerializeField] GameObject ReadyLamp;
    [SerializeField] GameObject NotReadyLamp;

    private bool lightsOn = true;

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            if (!LeaveDungeon())
            {
                Debug.LogWarning("Display can't leave dungeon yet");
            }
        }
        if (CanLeaveDungeon && !lightsOn && !GameSettings.Instance.PowerOutage)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            ReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            lightsOn = true;
        }
        else if (!CanLeaveDungeon && lightsOn && !GameSettings.Instance.PowerOutage)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            ReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            NotReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            lightsOn = false;
        }
        else if(GameSettings.Instance.PowerOutage && (ReadyLamp.GetComponentInChildren<Light>().intensity > 0 || NotReadyLamp.GetComponentInChildren<Light>().intensity > 0))
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            ReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }
    }
    public override void EnableInteractability()
    {
        inFocus = true;
        if (CanLeaveDungeon)
        {
            InteractionUIText.Instance.SetText("Press E to exit dungeon");
        }
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

}
