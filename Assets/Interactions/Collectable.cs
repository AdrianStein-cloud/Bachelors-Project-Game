using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collectable : Interactable
{
    public GameObject collectText;

    public Action onCollect;

    private void Start()
    {
        collectText = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault(g => g.name == "CollectText");
    }


    void Update()
    {
        if (collectText.activeInHierarchy && InputManager.Player.Interact.triggered)
        {
            onCollect?.Invoke();
            DisableInteractability();
        }
    }

    public override void EnableInteractability()
    {
        collectText.SetActive(true);
    }

    public override void DisableInteractability()
    {
        if(collectText) collectText.SetActive(false);
    }

}
