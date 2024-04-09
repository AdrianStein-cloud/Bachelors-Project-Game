using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UniversalForwardPlusVolumetric;

public class EventManager : MonoBehaviour
{
    private List<WeightedEvent> events;
    private GameObject flood;
    private VolumetricConfig volumetricConfig;
    private float defaultFogAttenuationDistance;
    private float defaultLocalScatteringIntensity;
    private bool guaranteeFlood = false;
    public static EventManager Instance;

    public void Init(GameObject flood, VolumetricConfig volumetricConfig)
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

        Instance = this;
    }

    public void SpawnRandomEvent(System.Random random)
    {
        FindObjectOfType<ElevatorRoom>().OnInDungeon -= EnableFlood;
        flood.SetActive(false);
        GameSettings.Instance.PowerOutage = false;
        ResetFog();
        if (!guaranteeFlood)
        {
            events.GetRollFromWeights(random)._event.Invoke();
        }
        else
        {
            Flooded();
        }
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
        if (GameSettings.Instance.Wave > 3 || guaranteeFlood)
        {
            FindObjectOfType<ElevatorRoom>().OnInDungeon += EnableFlood;
            GameSettings.Instance.Event = "Flooded!";
            guaranteeFlood = false;
        }
        else
        {
            NoEvent();
        }
    }

    void EnableFlood() => flood.SetActive(true);

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
        StartCoroutine(LerpFog(400, 30, 0.05f));
    }

    public void ResetFog()
    {
        if(isActiveAndEnabled) StartCoroutine(LerpFog(defaultFogAttenuationDistance, defaultLocalScatteringIntensity, 0f));
    }

    void OnDisable()
    {
        StopAllCoroutines();
        volumetricConfig.fogAttenuationDistance = defaultFogAttenuationDistance;
        volumetricConfig.localScatteringIntensity = defaultLocalScatteringIntensity;
        RenderSettings.fogDensity = 0f;
    }

    IEnumerator LerpFog(float attenuationDistance, float localScatteringIntensity, float fogDensity)
    {
        float elapsedTime = 0;
        var startAttenuationDistance = volumetricConfig.fogAttenuationDistance;
        var startLocalScatteringIntensity = volumetricConfig.localScatteringIntensity;
        var startFogDensity = RenderSettings.fogDensity;
        float smoothTime = 20f;
        while (elapsedTime < smoothTime)
        {
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, fogDensity, elapsedTime / smoothTime);
            volumetricConfig.fogAttenuationDistance = Mathf.Lerp(startAttenuationDistance, attenuationDistance, elapsedTime / smoothTime);
            volumetricConfig.localScatteringIntensity = Mathf.Lerp(startLocalScatteringIntensity, localScatteringIntensity, elapsedTime / smoothTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void GuaranteeFlood()
    {
        guaranteeFlood = true;
    }

    public void SetSizeOfDungeon(Vector3 size)
    {
        flood.transform.localScale = new Vector3(size.x/10, 1, size.z/10);
    }

    public void SetCenterOfDungeon(Vector3 center)
    {
        flood.transform.position = new Vector3(center.x, 3, center.z);
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
