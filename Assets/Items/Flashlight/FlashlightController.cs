using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : Item
{
    [SerializeField] float intensity;

    Light flashlight;
    new AudioSource audio;

    public BatteryItem batteryPrefab;
    public BatteryItem battery;
    public float batteryDrain;

    private void Awake()
    {
        battery = Instantiate(batteryPrefab, this.transform);
        battery.OnDead += () => flashlight.intensity = 0;

        flashlight = GetComponent<Light>();
        audio = GetComponent<AudioSource>();
    }

    public override void Primary() => ToggleFlashlight();

    public override void Select()
    {
        battery.Select();
    }

    public override void Deselect()
    {
        if (battery.On) ToggleFlashlight();
        battery.Deselect();
    }

    public void ToggleFlashlight()
    {
        audio.Play();

        battery.ToggleBattery(batteryDrain);

        flashlight.intensity = battery.On ? intensity : 0;
    }

    public void Upgrade(int intensity, int range, int angle, int batteryLife)
    {
        flashlight.range += range;

        flashlight.spotAngle += angle;

        this.intensity += intensity;
        if (battery.On) flashlight.intensity = this.intensity;
    }
}
