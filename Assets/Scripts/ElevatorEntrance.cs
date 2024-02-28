using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntrance : MonoBehaviour
{
    [SerializeField] GameObject ReadyLamp;
    [SerializeField] GameObject NotReadyLamp;

    private bool lightsOn = true;

    DungeonEntrance entrance;
    Animator anim;

    private void Start()
    {
        entrance = FindObjectOfType<DungeonEntrance>();
        anim = GetComponent<Animator>();
        FindObjectOfType<GameManager>().OnWaveOver += () => anim.SetTrigger("Close");
    }

    private void Update()
    {
        if (entrance.DungeonIsAvailable && !lightsOn)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            ReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            lightsOn = true;
            anim.SetTrigger("Open");
        }
        else if (!entrance.DungeonIsAvailable && lightsOn)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            ReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            NotReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            lightsOn = false;
        }
    }
}
