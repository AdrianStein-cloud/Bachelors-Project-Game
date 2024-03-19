using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood : MonoBehaviour
{
    [SerializeField] private float slowFactor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ISlowable slowable))
        {
            slowable.SlowDown(slowFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ISlowable slowable))
        {
            slowable.ResetSpeed();
        }
    }
}
