using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public float interactRange;

    public LayerMask interactableMask;

    Interactable hoveredInteractable = null;


    Vector3 start;
    Vector3 end;

    private void Update()
    {
        CheckForInteraction();
    }


    void CheckForInteraction()
    {       
        Vector3 dir = transform.rotation * Vector3.forward;

        RaycastHit hit;
        //Debug.DrawLine(transform.position, transform.position + dir.normalized * interactRange, Color.red);
        if (Physics.Raycast(transform.position, dir, out hit, interactRange, interactableMask))
        {
            //Debug.Log(hit.transform.name);
            var newInteractable = hit.transform.GetComponent<Interactable>();


            /*if (newInteractable == null)
            {
                //Debug.LogError("Couldn't find interactable script on object");
                hoveredInteractable?.DisableInteractability();
                return;
            }*/

            if (hoveredInteractable != newInteractable)
            {
                hoveredInteractable?.DisableInteractability();
                newInteractable?.EnableInteractability();
                hoveredInteractable = newInteractable;
            }
        }
        else
        {
            hoveredInteractable?.DisableInteractability();
            hoveredInteractable = null;
        }
    }
}
