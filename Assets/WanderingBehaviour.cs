using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : MonoBehaviour
{
    Animator anim;
    public WanderingState state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        GetComponent<BehaviorExecutor>().SetBehaviorParam("target", GameObject.FindGameObjectWithTag("Player"));
    }

    public void UpdateState(WanderingState newstate)
    {
        if (state != newstate)
        {
            Debug.Log("Setting: " + state.ToString() + " to false.");
            anim.SetBool(state.ToString(), false);

            state = newstate;

            Debug.Log("Setting: " + state.ToString() + " to true.");
            anim.SetBool(state.ToString(), true);

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
