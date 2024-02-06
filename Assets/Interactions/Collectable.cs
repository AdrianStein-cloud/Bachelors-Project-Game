using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public GameObject collectText;


    void Update()
    {
        if (collectText.activeInHierarchy && InputManager.Player.Interact.triggered)
        {
            Debug.LogWarning("Collect not yet implemented");
        }
    }

    public override void EnableInteractability()
    {
        collectText.SetActive(true);
    }

    public override void DisableInteractability()
    {
        collectText.SetActive(false);
    }

}
