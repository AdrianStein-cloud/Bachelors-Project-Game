using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : Interactable
{
    public bool DungeonIsAvailable { get; set; } = false;
    public Action EnterDungeon { get; set; }
    public Action LeaveDungeon { get; set; }
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
            if (DungeonIsAvailable && !GameSettings.Instance.PlayerInDungeon)
            {
                GameSettings.Instance.PlayerInDungeon = true;
                StartCoroutine(Enter());
                EnterDungeon();
            }
            else if (FindObjectOfType<ElevatorExit>().CanLeaveDungeon && GameSettings.Instance.PlayerInDungeon)
            {
                DungeonIsAvailable = false;
                GameSettings.Instance.PlayerInDungeon = false;
                FindObjectOfType<ElevatorExit>().CanLeaveDungeon = false;
                StartCoroutine(Exit());
            }
        }
    }

    IEnumerator Enter()
    {
        elevator.ToggleEntranceElevator(false);
        yield return new WaitForSeconds(1f);
        elevator.Enter();
    }

    IEnumerator Exit()
    {
        elevator.ToggleExitElevator(false);
        yield return new WaitForSeconds(1f);
        elevator.Exit();
        LeaveDungeon();
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        var exit = FindObjectOfType<ElevatorExit>();

        if (exit != null && GameSettings.Instance.PlayerInDungeon)
        {
            if (exit.CanLeaveDungeon)
            {
                InteractionUIText.Instance.SetText("Press E to exit dungeon");
            }
            else InteractionUIText.Instance.SetText("");
        }
        else InteractionUIText.Instance.SetText($"Press E to enter dungeon");
        
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }
}
