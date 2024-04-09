using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class FlashlightController : Item
{
    [SerializeField] float intensity;

    Light flashlight;
    new AudioSource audio;

    public BatteryItem batteryPrefab;
    public BatteryItem battery;
    public float batteryDrain;
    public GameObject hand;
    public Animator handAnim;

    private void Awake()
    {
        hand = Camera.main.transform.Find("FlashlightHand").gameObject;
        hand.GetComponent<FlashlightAnimEvents>().controller = this;
        battery = Instantiate(batteryPrefab, this.transform);
        battery.OnDead += () => flashlight.intensity = 0;
        handAnim = hand.GetComponent<Animator>();

        flashlight = hand.GetComponentInChildren<Light>();
        audio = GetComponent<AudioSource>();
    }

    public override void Primary() => ToggleFlashlight();

    public override void Select()
    {
        battery.Select();
        hand.SetActive(true);
        handAnim.SetBool("Equip", true);
    }

    public override void Deselect()
    {
        if (battery.On)
        {
            battery.ToggleBattery(batteryDrain);
            TryToggleLight();
        }
        battery.Deselect();
        handAnim.SetBool("Equip", false);
    }

    public void ToggleFlashlight()
    {
        audio.Play();

        battery.ToggleBattery(batteryDrain);
        handAnim.SetTrigger("Click");
    }

    public void Upgrade(int intensity, int range, int angle, int batteryLife)
    {
        flashlight.range += range;

        flashlight.spotAngle += angle;

        this.intensity += intensity;
        if (battery.On) flashlight.intensity = this.intensity;
    }

    public void TryToggleLight()
    {
        flashlight.intensity = battery.On ? intensity : 0;
    }
}
