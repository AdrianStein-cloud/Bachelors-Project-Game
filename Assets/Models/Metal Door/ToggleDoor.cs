using BBUnity.Actions;
using BBUnity.Conditions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    public bool isLocked = false;
    public bool open = false;

    public float openDist;

    Animator anim;
    GameObject player;
    GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Interact();
    }

    public void Interact()
    {
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
    }

    void Close()
    {
        open = false;
        anim.SetTrigger("Toggle");
    }
}
