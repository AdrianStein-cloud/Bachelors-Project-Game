using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : Interactable
{
    public bool DungeonIsAvailable { get; set; } = false;
    public Action EnterDungeon { get; set; }
    bool inFocus = false;
    ElevatorRoom elevator;

    private void Start()
    {
        elevator = FindObjectOfType<ElevatorRoom>();
    }

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            if (DungeonIsAvailable)
            {
                StartCoroutine(Enter());
                EnterDungeon();
            }
            else
            {
                Debug.LogWarning("Display dungeon not yet available");
            }
        }
    }

    IEnumerator Enter()
    {
        elevator.ToggleEntranceElevator(false);
        yield return new WaitForSeconds(1f);
        elevator.Enter();
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        InteractionUIText.Instance.SetText("Press E to enter dungeon");
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

}
