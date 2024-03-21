using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeComputer : Interactable
{
    private bool inFocus;
    UpgradeManager upgradeManager;

    private void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            upgradeManager.DisplayUpgrades();
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
        InteractionUIText.Instance.SetText("Press E to buy upgrades");
    }
}
