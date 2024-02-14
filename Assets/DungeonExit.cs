using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : Interactable
{
    public Func<bool> LeaveDungeon { get; set; }
    bool inFocus = false;

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            if (!LeaveDungeon())
            {
                Debug.LogWarning("Display can't leave dungeon yet");
            }
        }
    }
    public override void EnableInteractability()
    {
        inFocus = true;
        InteractionUIText.Instance.SetText("Press E to exit dungeon");
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

}
