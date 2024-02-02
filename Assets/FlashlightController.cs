using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : Item
{
    Light flashlight;
    float normalIntensity;
    bool on = false;

    private void Start()
    {
        flashlight = GetComponent<Light>();
        normalIntensity = flashlight.intensity;
    }

    public override void Primary() => ToggleFlashlight();

    public void ToggleFlashlight()
    {
        if (on)
        {
            on = false;
            flashlight.intensity = 0;
        }
        else
        {
            on = true;
            flashlight.intensity = normalIntensity;
        }
    }
}
