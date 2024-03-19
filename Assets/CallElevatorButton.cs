using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CallElevatorButton : Interactable
{
    [SerializeField] AudioSource clickSource;
    bool inFocus = false;

    private void Update()
    {
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
