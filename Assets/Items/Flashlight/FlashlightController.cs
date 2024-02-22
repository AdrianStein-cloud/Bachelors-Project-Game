using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : Item
{
    [SerializeField] float intensity;
    [SerializeField] float batteryLife;
    [SerializeField] float batteryDrain;

    Light flashlight;
    float currentBatteryLife;
    GameObject flashlightBar;
    Image flashlightFill;
    bool on = false;
    bool dead;
    new AudioSource audio;

    private void Awake()
    {
        flashlight = GetComponent<Light>();
        audio = GetComponent<AudioSource>();
        currentBatteryLife = batteryLife;
        FindObjectOfType<GameManager>().OnWaveOver += () =>
        {
            currentBatteryLife = batteryLife;
            UpdateBar();
        };
        flashlightBar = FindObjectOfType<Canvas>().transform.Find("FlashlightBar").gameObject;
        flashlightFill = flashlightBar.transform.Find("Fill").GetComponent<Image>();
    }

    private void Update()
    {
        if (dead) return;
        if (on)
        {
            currentBatteryLife -= batteryDrain * Time.deltaTime;
            if (currentBatteryLife <= 0)
            {
                dead = true;
                on = false;
                flashlight.intensity = 0;
                currentBatteryLife = 0;
            }

            UpdateBar();
        }
    }

    public override void Primary() => ToggleFlashlight();

    public override void Select()
    {
        flashlightBar.SetActive(true);
        UpdateBar();
    }

    public override void Deselect()
    {
        flashlightBar.SetActive(false);
    }

    public void ToggleFlashlight()
    {
        audio.Play();
        if (dead) return;

        if (on)
        {
            on = false;
            flashlight.intensity = 0;
        }
        else
        {
            on = true;
            flashlight.intensity = intensity;
        }
    }

    private void UpdateBar() => flashlightFill.fillAmount = currentBatteryLife / batteryLife;

    public void UpgradeLigthing(int intensity, int range)
    {
        this.intensity += intensity;
        flashlight.range += range;
        if(on) flashlight.intensity = this.intensity;
    }
}
