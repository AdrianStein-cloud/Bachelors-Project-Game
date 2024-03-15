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
    private float intensity;

    DungeonEntrance entrance;
    Animator anim;

    private void Start()
    {
        entrance = FindObjectOfType<DungeonEntrance>();
        anim = GetComponent<Animator>();
        FindObjectOfType<GameManager>().OnWaveOver += () =>
        {
            anim.SetTrigger("Close");
            timer = Time.time;
        };
        //bellSound = GetComponent<AudioSource>();
        timer = Time.time;
        intensity = ReadyLamp.GetComponentInChildren<Light>().intensity;
    }

    private void Update()
    {
        if (entrance.DungeonIsAvailable && !lightsOn && timer + minimumTime < Time.time)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = intensity;
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
            NotReadyLamp.GetComponentInChildren<Light>().intensity = intensity;
            NotReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            lightsOn = false;
        }
    }
}
