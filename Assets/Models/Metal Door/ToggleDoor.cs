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
    public bool canInteract;
    public float delay;
    private float lastInteract;

    bool inFocus = false;

    Animator anim;
    GameObject player;
    GameObject cam;
    AudioSource audioSource;

    public AudioClip openSound, closeSound;

    private NavMeshObstacle navObstacle;

    public bool otherway;

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
        if (lastInteract + delay <= Time.time) canInteract = true;
    }

    public void Interact(GameObject user)
    {
        otherway = Vector3.Angle(user.transform.forward, transform.parent.parent.transform.right) > 80;
        anim.SetBool("Otherway", Vector3.Angle(user.transform.forward, transform.right) > 80);
        if (canInteract && !isLocked)
        {
            canInteract = false;
            lastInteract = Time.time;
            anim.SetTrigger("Toggle");
            //navObstacle.enabled = true;

            if (user == player) audioSource.volume = 0.5f;
            else audioSource.volume = 1f;

            audioSource.clip = open ? closeSound : openSound;

            audioSource.Play();
            open = !open;
        }
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
