using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntrance : MonoBehaviour
{
    [SerializeField] GameObject ReadyLamp;
    [SerializeField] GameObject NotReadyLamp;
    [SerializeField] float minimumTime = 4f;
    [SerializeField] AudioSource bellSound;

    private bool lightsOn = true;
    private float timer;

    ElevatorButton elevatorButton;
    ElevatorRoom elevatorRoom;
    Animator anim;

    private void Start()
    {
        elevatorButton = FindObjectOfType<ElevatorButton>();
        elevatorRoom = FindObjectOfType<ElevatorRoom>();
        anim = GetComponent<Animator>();
        FindObjectOfType<GameManager>().OnWaveOver += () =>
        {
            timer = Time.time;
        };
        //bellSound = GetComponent<AudioSource>();
        timer = Time.time;
    }

    private void Update()
    {
        if (elevatorButton.DungeonIsAvailable && !lightsOn && timer + minimumTime < Time.time)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            ReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            lightsOn = true;
            bellSound.Play();
            anim.SetTrigger("Open");
            elevatorRoom.ToggleEntranceElevator(true);
        }
        else if (!elevatorButton.DungeonIsAvailable && lightsOn)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            ReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            NotReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            lightsOn = false;
        }
    }

    public void ToggleElevator(bool open)
    {
        anim.SetTrigger(open ? "Open" : "Close");
    }
}
