using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BatteryItem : Item
{
    float batteryDrain;

    public float startBatteryLife;
    float MaxBatteryLife 
    {
        get
        {
            return startBatteryLife * (1 + Stats.Instance.eletronics.batteryLifeMultiplier);
        }
    }
    public bool On { get; private set; }
    public Action OnDead;

    public bool Dead => currentBatteryLife <= 0;

    GameObject powerBar;
    GameObject PowerBar { get
        {
            if(powerBar == null)
            {
                powerBar = GameSettings.Instance.canvas.transform.Find("PowerBar").gameObject;
            }
            return powerBar;
        }
    }
    Image flashlightFill;
    Image FlashlightFill
    {
        get
        {
            if (flashlightFill == null)
            {
                flashlightFill = flashlightFill = powerBar.transform.Find("Fill").GetComponent<Image>(); ;
            }
            return flashlightFill;
        }
    }
    float currentBatteryLife;

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
        /*powerBar = GameSettings.Instance.canvas.transform.Find("PowerBar").gameObject;
        flashlightFill = powerBar.transform.Find("Fill").GetComponent<Image>();*/
        FillBattery();

    }

    private void Start()
    {
        Stats.Instance.eletronics.rechargeBattery += (fraction) =>
        {
            currentBatteryLife = Mathf.Min(currentBatteryLife + MaxBatteryLife * fraction, MaxBatteryLife);
            UpdateBar();
        };
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
        PowerBar.SetActive(true);
    }

    public override void Deselect()
    {
        selected = false;
        PowerBar.SetActive(false);
    }

    public void FillBattery()
    {
        currentBatteryLife = MaxBatteryLife;
    }

    private void UpdateBar() => FlashlightFill.fillAmount = currentBatteryLife / MaxBatteryLife;
}
