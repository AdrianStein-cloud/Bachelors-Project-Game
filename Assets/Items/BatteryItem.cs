using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BatteryItem : Item
{
    float batteryDrain;
    public float batteryLife;
    public bool On { get; private set; }
    public Action OnDead;

    public bool Dead => currentBatteryLife <= 0;

    GameObject powerBar;
    float currentBatteryLife;
    Image flashlightFill;
    bool selected;

    private void Awake()
    {
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnWaveOver += () =>
            {
                FillBattery();
            };
        }

        powerBar = GameSettings.Instance.canvas.transform.Find("PowerBar").gameObject;
        flashlightFill = powerBar.transform.Find("Fill").GetComponent<Image>();
        FillBattery();
    }

    private void Update()
    {
        if (On)
        {
            currentBatteryLife -= batteryDrain * Time.deltaTime;
            if (currentBatteryLife <= 0)
            {
                On = false;
                currentBatteryLife = 0;
                OnDead?.Invoke();
            }

        }
        if (selected) UpdateBar();
    }

    public void ToggleBattery(float drain)
    {
        if (Dead) return;

        batteryDrain = drain;

        On = !On;
    }

    public override void Select()
    {
        selected = true;
        powerBar.SetActive(true);
    }

    public override void Deselect()
    {
        selected = false;
        powerBar.SetActive(false);
    }

    public void FillBattery()
    {
        currentBatteryLife = batteryLife;
    }

    private void UpdateBar() => flashlightFill.fillAmount = currentBatteryLife / batteryLife;

    public void UpgradeBattery(int batteryLife)
    {
        this.batteryLife += batteryLife;
        currentBatteryLife = this.batteryLife;
    }
}
