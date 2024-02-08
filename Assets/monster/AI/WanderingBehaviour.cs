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
}

public enum WanderingState
{
    Wander,
    Search,
    Chase,
    Attack,
    Dead
}
