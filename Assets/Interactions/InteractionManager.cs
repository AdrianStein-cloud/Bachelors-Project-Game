using System;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public float interactRange;

    public LayerMask interactableMask;

    Interactable hoveredInteractable = null;


    Vector3 start;
    Vector3 end;

    private void FixedUpdate()
    {
        CheckForInteraction();
    }


    void CheckForInteraction()
    {       
        Vector3 dir = transform.rotation * Vector3.forward;

        var hits = Physics.RaycastAll(transform.position, dir, interactRange, interactableMask);

        var interatable = hits.Select(hit => hit.collider.transform.GetComponent<Interactable>()).FirstOrDefault(i => i != null);
        if (interatable != null)
        {
            if (hoveredInteractable != interatable)
            {
                hoveredInteractable?.DisableInteractability();
                interatable?.EnableInteractability();
                hoveredInteractable = interatable;
            }
        }
        else
        {
            hoveredInteractable?.DisableInteractability();
            hoveredInteractable = null;
        }
    }
}
