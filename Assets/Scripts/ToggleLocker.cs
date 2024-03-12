using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleLocker : Interactable
{
    [SerializeField] AudioClip openSound, closeSound;
    [SerializeField] float delay;

    Animator anim;
    AudioSource source;
    float lastInteract;
    bool inFocus;
    bool open;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        source = GetComponentInParent<AudioSource>();
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

        anim.SetTrigger(open ? "On" : "Off");
        source.PlayOneShot(open ? openSound : closeSound);
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
