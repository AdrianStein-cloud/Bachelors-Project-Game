using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CallElevatorButton : Interactable
{
    [SerializeField] AudioSource clickSource;
    bool inFocus = false;
    DungeonExit exit;

    private void Awake()
    {
        exit = GetComponentInParent<DungeonExit>();
    }

    private void Update()
    {
        if (exit != null) { 
            if (exit.CanLeaveDungeon)
            {
                GetComponent<BoxCollider>().enabled = false;
                exit = null;
            }
        }

        if (InputManager.Player.Interact.triggered && inFocus)
        {
            clickSource.PlayOneShot(clickSource.clip);
        }
    }

    public override void DisableInteractability()
    {
        inFocus = false;
    }

    public override void EnableInteractability()
    {
        inFocus = true;
    }
}
