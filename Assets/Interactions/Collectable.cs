using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collectable : Interactable
{
    public Action onCollect;

    bool inFocus;

    void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            onCollect();
            gameObject.SetActive(false);
        }
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        InteractionUIText.Instance.SetText("Press E to pickup ball");

    }

    public override void DisableInteractability()
    {
        InteractionUIText.Instance.SetText("");
    }

}
