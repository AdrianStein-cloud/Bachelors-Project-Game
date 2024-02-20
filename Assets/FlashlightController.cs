using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : Item
{
    Light flashlight;
    float normalIntensity;
    bool on = false;
    AudioSource audio;

    private void Start()
    {
        flashlight = GetComponent<Light>();
        normalIntensity = flashlight.intensity;
        audio = GetComponent<AudioSource>();
    }

    public override void Primary() => ToggleFlashlight();

    public void ToggleFlashlight()
    {
        if (on)
        {
            on = false;
            flashlight.intensity = 0;
            audio.Play();
        }
        else
        {
            on = true;
            flashlight.intensity = normalIntensity;
            audio.Play();
        }
    }

    public void UpgradeLigthing(int intensity, int range)
    {
        normalIntensity += intensity;
        flashlight.range += range;
        if(on) flashlight.intensity = normalIntensity;
    }
}
