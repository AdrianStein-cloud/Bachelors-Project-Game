using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private List<GameObject> valuables;
    private bool isOpen = false;
    private Animator animator;
    private bool inFocus = false;

    private void Start()
    {
        valuables.ForEach(x => x.SetActive(isOpen));
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered && Stats.Instance.player.keysHeld > 0)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        Destroy(GetComponent<BoxCollider>());
        Stats.Instance.player.keysHeld--;
        animator.SetTrigger("Open");
        isOpen = true;
        valuables.ForEach(x => x.SetActive(isOpen));
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        if (!isOpen)
        {
            if (Stats.Instance.player.keysHeld > 0)
                InteractionUIText.Instance.SetText("Open Chest");
            else
                InteractionUIText.Instance.SetText("Chest Locked");
        }
    }
}
