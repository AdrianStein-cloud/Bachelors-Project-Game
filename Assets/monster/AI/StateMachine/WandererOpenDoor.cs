using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class WandererOpenDoor : StateInterrupt
{
    public float openDoorAnimationDelay;
    public float openDoorDelay;

    WandererInfo info;
    WandererMovement movement;
    Animator anim;

    private void Awake()
    {
        info = GetComponent<WandererInfo>();
        anim = GetComponent<Animator>();
        movement = GetComponent<WandererMovement>();
    }

    private void OnEnable()
    {
        Debug.Log("Opening door");
        StartCoroutine(OpenDoorWait());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator OpenDoorWait()
    {
        /*var prevSpeed = agent.speed;
        agent.speed = 0;*/

        movement.Stop();
        anim.SetTrigger("Search");

        yield return new WaitForSeconds(openDoorAnimationDelay);

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

        info.DoorToOpen = null;
        Done();

        //THIS NEEDS TO BE UPDATED BASED ON THE PREVIOUS STATE
        /*anim.SetTrigger("Wander");

        agent.speed = prevSpeed;*/

    }
}
