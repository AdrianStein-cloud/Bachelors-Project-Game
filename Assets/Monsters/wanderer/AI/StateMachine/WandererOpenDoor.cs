using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class WandererOpenDoor : StateInterrupt
{
    public float openDoorAnimationDelay;
    public float openDoorDelay;

    WandererMovement movement;
    WandererInfo info;
    Animator anim;

    private void Awake()
    {
        movement = GetComponent<WandererMovement>();
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //Debug.Log("Opening door");
        StartCoroutine(OpenDoorWait());
    }

    private void OnDisable()
    {
        anim.SetBool("Search", false);
        StopAllCoroutines();
    }

    IEnumerator OpenDoorWait()
    {
        /*var prevSpeed = agent.speed;
        agent.speed = 0;*/

        movement.Stop();
        if (!info.IsChasing)
        {
            anim.SetBool("Search", true);

            yield return new WaitForSeconds(openDoorAnimationDelay);

            anim.SetBool("Search", false);
        }

        var door = info.DoorToOpen.GetComponent<ToggleDoor>();
        //Coutnermeasure against the player opening the door during wait
        if (!door.open)
        {

            anim.SetTrigger("Attack");

            yield return new WaitForSeconds(openDoorDelay);

            //Coutnermeasure again
            if (!door.open)
            {
                door.Interact(gameObject);
            }
        }
        //anim.SetBool("Search", false);

        info.DoorToOpen = null;
        Done();

        //THIS NEEDS TO BE UPDATED BASED ON THE PREVIOUS STATE
        /*anim.SetTrigger("Wander");

        agent.speed = prevSpeed;*/

    }
}
