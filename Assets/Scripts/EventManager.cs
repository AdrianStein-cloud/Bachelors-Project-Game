using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniversalForwardPlusVolumetric;

public class EventManager
{
    private List<WeightedEvent> events;
    private GameObject flood;
    private VolumetricConfig volumetricConfig;
    private float defaultFogAttenuationDistance;
    private float defaultLocalScatteringIntensity;

    public EventManager(GameObject flood, VolumetricConfig volumetricConfig)
    {
        this.flood = flood;
        this.volumetricConfig = volumetricConfig;
        flood.SetActive(false);

        defaultFogAttenuationDistance = volumetricConfig.fogAttenuationDistance;
        defaultLocalScatteringIntensity = volumetricConfig.localScatteringIntensity;

        events = new List<WeightedEvent>
        {
            new WeightedEvent(Flooded, 100),
            new WeightedEvent(PowerOutage, 100),
            new WeightedEvent(Foggy, 100),
            new WeightedEvent(NoEvent, 200)
        };
    }

    public void SpawnRandomEvent(System.Random random)
    {
        flood.gameObject.SetActive(false);
        GameSettings.Instance.PowerOutage = false;
        ResetFog();
        events.GetRollFromWeights(random)._event.Invoke();
    }

    private void PowerOutage()
    {
        if(GameSettings.Instance.Wave > 2)
        {
            GameSettings.Instance.Event = "Power Outage!";
            GameSettings.Instance.PowerOutage = true;
            GameSettings.Instance.LightFailPercentage = 100;
        }
        else
        {
            NoEvent();
        }
    }

    private void Flooded()
    {
        if (GameSettings.Instance.Wave > 3)
        {
            flood.SetActive(true);
            GameSettings.Instance.Event = "Flooded!";
        }
        else
        {
            NoEvent();
        }
    }

    private void Foggy()
    {
        if (GameSettings.Instance.Wave > 2)
        {
            GameSettings.Instance.Event = "Foggy!";
            SetFoggy();
        }
        else
        {
            NoEvent();
        }
    }

    private void NoEvent()
    {
        GameSettings.Instance.Event = null;
    }

    private void SetFoggy()
    {
        volumetricConfig.fogAttenuationDistance = 160;
        volumetricConfig.localScatteringIntensity = 40;
    }

    public void ResetFog()
    {
        Debug.Log("defaultFogAttenuationDistance: " + defaultFogAttenuationDistance);
        Debug.Log("defaultLocalScatteringIntensity: " + defaultLocalScatteringIntensity);
        volumetricConfig.fogAttenuationDistance = defaultFogAttenuationDistance;
        volumetricConfig.localScatteringIntensity = defaultLocalScatteringIntensity;
    }

    public void SetSizeOfDungeon(Vector3 size)
    {
        if (flood.activeInHierarchy)
        {
            flood.transform.localScale = new Vector3(size.x/10, 1, size.z/10);
        }
    }

    public void SetCenterOfDungeon(Vector3 center)
    {
        if (flood.activeInHierarchy)
        {
            flood.transform.position = new Vector3(center.x, 3, center.z);
        }
    }
}

public class WeightedEvent : IWeighted
{
    public WeightedEvent(Action _event, int Weight)
    {
        this.Weight = Weight;
        this._event = _event;
    }

    public Action _event;
    [field: SerializeField] public int Weight { get; set; }
}
