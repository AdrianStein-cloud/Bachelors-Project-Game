using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntrance : MonoBehaviour
{
    [SerializeField] GameObject ReadyLamp;
    [SerializeField] GameObject NotReadyLamp;
    [SerializeField] float minimumTime = 4f;

    private bool lightsOn = true;
    private float timer;

    DungeonEntrance entrance;
    Animator anim;
    AudioSource bellSound;

    private void Start()
    {
        entrance = FindObjectOfType<DungeonEntrance>();
        anim = GetComponent<Animator>();
        FindObjectOfType<GameManager>().OnWaveOver += () =>
        {
            anim.SetTrigger("Close");
            timer = Time.time;
        };
        bellSound = GetComponent<AudioSource>();
        timer = Time.time;
    }

    private void Update()
    {
        if (entrance.DungeonIsAvailable && !lightsOn && timer + minimumTime < Time.time)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            ReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            lightsOn = true;
            bellSound.Play();
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
