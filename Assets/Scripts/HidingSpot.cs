using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public GameObject FoundLocation { get; private set; }

    private void Awake()
    {
        FoundLocation = transform.Find("FoundLocation").gameObject;
    }

    public void ArrivedAtFoundLocation()
    {
        //Idea was locker door could open here (probably don't have time for it)
        Debug.LogWarning("Not implemented");
    }
}
