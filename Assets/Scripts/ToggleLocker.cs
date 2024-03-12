using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleLocker : Interactable
{
    [SerializeField] float delay;

    Animator anim;
    float lastInteract;
    bool inFocus;
    bool open;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        InputManager.Player.Interact.performed += Interact;
        lastInteract = Time.time;
    }

    private void OnDestroy()
    {
        InputManager.Player.Interact.performed -= Interact;
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (!inFocus || lastInteract + delay > Time.time) return;

        lastInteract = Time.time;
        open = !open;

        if (open) anim.SetTrigger("On");
        else anim.SetTrigger("Off");
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
