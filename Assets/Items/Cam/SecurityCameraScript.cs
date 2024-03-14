using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraScript : MonoBehaviour
{
    public bool colliding;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Room")) colliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Room")) colliding = false;
    }
}
