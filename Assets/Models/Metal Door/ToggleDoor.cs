using BBUnity.Actions;
using BBUnity.Conditions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToggleDoor : Interactable
{
    public bool isLocked = false;
    public bool open = false;

    bool inFocus = false;

    public float openDist;

    Animator anim;
    GameObject player;
    GameObject cam;
    AudioSource audioSource;

    private NavMeshObstacle navObstacle;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = transform.parent.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.gameObject;
        navObstacle = GetComponentInChildren<NavMeshObstacle>();
        if (!isLocked) navObstacle.enabled = false;
    }

    private void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered) Interact(player);
    }

    public void Interact(GameObject user)
    {
        anim.SetBool("Otherway", Vector3.Angle(user.transform.forward, transform.right) > 80);
        if (!isLocked)
        {
            if (!open) Open();
            else Close();
        }
    }

    void Open()
    {
        open = true;
        anim.SetTrigger("Toggle");
        navObstacle.enabled = true;
        audioSource.Play();
    }

    void Close()
    {
        open = false;
        navObstacle.enabled = false;
        anim.SetTrigger("Toggle");
    }

    public override void EnableInteractability()
    {
        inFocus = true;
    }

    public override void DisableInteractability()
    {
        inFocus = false;
    }
}
