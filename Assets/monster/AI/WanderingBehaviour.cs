using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : MonoBehaviour
{
    Animator anim;
    public WanderingState state;

    public List<GameObject> roomsCopy;
    public GameObject wanderingTo;
    public bool once = false;
    public GameObject doorToOpen;

    public GameObject currentRoom;

    public bool hasReachedRoom = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //GetComponent<BehaviorExecutor>().SetBehaviorParam("target", GameObject.FindGameObjectWithTag("Player"));
    }

    public void UpdateState(WanderingState newstate)
    {
        state = newstate;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.gameObject;
        }
    }
}

public enum WanderingState
{
    Wander,
    Search,
    Chase,
    Attack,
    Dead
}
