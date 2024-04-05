using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    private bool inFocus = false;

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            Stats.Instance.player.AddKey();
            Destroy(gameObject);
        }
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        InteractionUIText.Instance.SetText("Pickup Key");
    }
}
