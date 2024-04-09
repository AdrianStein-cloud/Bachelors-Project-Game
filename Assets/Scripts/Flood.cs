using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flood : MonoBehaviour
{
    [SerializeField] private float slowFactor;

    private void OnDisable()
    {
        foreach (var slowable in FindObjectsOfType<MonoBehaviour>().OfType<ISlowable>())
        {
            slowable.ResetSpeed(slowFactor);
        }
    }

    private void Start()
    {
        FindObjectOfType<ElevatorButton>().LeaveDungeon += () => gameObject.SetActive(false);
    }

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
            slowable.ResetSpeed(slowFactor);
        }
    }
}
