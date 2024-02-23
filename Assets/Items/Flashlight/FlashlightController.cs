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
        Debug.Log("Awake 1");
        flashlight = GetComponent<Light>();
        Debug.Log("Awake 2");
        audio = GetComponent<AudioSource>();
        Debug.Log("Awake 3");
        currentBatteryLife = batteryLife;
        Debug.Log("Awake 4");
        var gameManager = FindObjectOfType<GameManager>();
        Debug.Log("Awake 5");
        if (gameManager != null)
        {
            gameManager.OnWaveOver += () =>
            {
                currentBatteryLife = batteryLife;
                UpdateBar();
            };
        }
        Debug.Log("Awake 6");
        flashlightBar = GameObject.Find("Canvas")
            .transform
            .Find("FlashlightBar")
            .gameObject;
        Debug.Log("Awake 7");
        flashlightFill = flashlightBar.transform.Find("Fill").GetComponent<Image>();
        Debug.Log($"Bar Awake: {flashlightBar}");
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
        Debug.Log($"Bar Select: {flashlightBar}");
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

    public void Upgrade(int intensity, int range, int angle, int batteryLife)
    {
        flashlight.range += range;

        flashlight.spotAngle += angle;

        this.intensity += intensity;
        if(on) flashlight.intensity = this.intensity;


        this.batteryLife += batteryLife;
        currentBatteryLife = this.batteryLife;
        UpdateBar();
    }
}
