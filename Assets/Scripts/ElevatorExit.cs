using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorExit : MonoBehaviour
{
    [SerializeField] GameObject ReadyLamp;
    [SerializeField] GameObject NotReadyLamp;

    public bool CanLeaveDungeon { get; set; } = false;

    private bool lightsOn = true;
    private bool doorOpen = false;
    private Animator anim;
    AudioSource openCloseSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        openCloseSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (CanLeaveDungeon && !lightsOn)
        {
            ReadyLamp.GetComponentInChildren<Light>().intensity = 20f;
            ReadyLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            NotReadyLamp.GetComponentInChildren<Light>().intensity = 0f;
            NotReadyLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            lightsOn = true;
            FindObjectOfType<ElevatorRoom>().ToggleExitElevator(true);
        }
        else if (!CanLeaveDungeon && lightsOn)
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
        openCloseSound.Play();
        doorOpen = open;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && doorOpen && !CanLeaveDungeon && GameSettings.Instance.PlayerInDungeon)
        {
            doorOpen = false;
            FindObjectOfType<ElevatorRoom>().ToggleExitElevator(false);
        }
    }
}
