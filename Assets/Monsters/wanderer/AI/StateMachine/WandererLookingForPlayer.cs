using System.Collections;
using UnityEngine;

public class WandererLookingForPlayer : StateProcess<WandererState>
{
    WandererMovement movement;
    WandererSight sight;
    WandererInfo info;
    Animator anim;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        sight = GetComponent<WandererSight>();
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(SearchRoom());
    }

    private void OnDisable()
    {
        anim.SetBool("Search", false);
        anim.SetBool("Wander", false);
        StopAllCoroutines();
    }

    private void Update()
    {
        if (sight.IsPlayerInSight)
        {
            anim.SetBool("Search", false);
            anim.SetBool("Wander", false);
            StateController.SwitchState(WandererState.Chase);
        }
    }

    IEnumerator SearchRoom()
    {
        movement.Stop();
        anim.SetBool("Search", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("Search", false);
        if (enabled)
        {
            anim.SetBool("Wander", true);
            var currentRoom = info.CurrentRoom.GetComponentInChildren<Room>();
            var center = currentRoom.centerObject.transform.position;
            float speed = GetComponent<WandererRoam>().speed;
            movement.MoveTo(center, speed, DestinationReached);
        }

    }

    void DestinationReached()
    {
        if(enabled)
        {
            StateController.SwitchState(WandererState.Roam);
        }
    }
}
