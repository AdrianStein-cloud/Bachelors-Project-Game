using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    Light flashlight;
    float normalIntensity;
    bool on = false;

    private void Start()
    {
        flashlight = GetComponent<Light>();
        normalIntensity = flashlight.intensity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleFlashlight();
        }
    }

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
