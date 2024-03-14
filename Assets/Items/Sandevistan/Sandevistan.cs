using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Sandevistan : Item
{
    private GameObject distanceCounter;
    private TextMeshProUGUI timerText;
    private float timer = 0f;
    public float cooldown;
    public float duration;
    public float timeScale;
    private CameraController cameraController;

    private void Awake()
    {
        distanceCounter = GameSettings.Instance.canvas.transform.Find("Distance Counter").gameObject;
        timerText = distanceCounter.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            timerText.text = timer.ToString("F1") + " s";
            if (timer < 0.5) timerText.text = string.Empty;
        }
    }

    public override void Primary() => TrySlowTime();

    private void TrySlowTime()
    {
        if (timer <= 0)
        {
            timer = cooldown;
            StartCoroutine(SlowTime());
        }
    }

    IEnumerator SlowTime()
    {
        float fov = Camera.main.fieldOfView;
        StartCoroutine(FadeIn(fov));
        yield return new WaitForSecondsRealtime(duration);
        
        StartCoroutine(FadeOut(fov));
    }

    IEnumerator FadeIn(float fov)
    {
        float elapsedTime = 0f;
        var startValue = 1f;
        float smoothTime = 1f;
        PostProcessingHandler.Instance.SetBloom(smoothTime, 30);
        PostProcessingHandler.Instance.SetChromaticAberration(smoothTime, 1f);
        PostProcessingHandler.Instance.SetSaturation(smoothTime, 100f);
        while (elapsedTime < smoothTime)
        {
            Time.timeScale = Mathf.Lerp(startValue, timeScale, elapsedTime / smoothTime);
            cameraController.SetFov(Mathf.Lerp(fov, fov + 20, elapsedTime / smoothTime));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOut(float fov)
    {
        Time.timeScale = 1f;
        cameraController.SetFov(fov);

        float elapsedTime = 0f;
        float smoothTime = 1f;
        PostProcessingHandler.Instance.ResetBloom(smoothTime);
        PostProcessingHandler.Instance.SetChromaticAberration(smoothTime);
        PostProcessingHandler.Instance.ResetSaturation(smoothTime);
        while (elapsedTime < smoothTime)
        {
            Time.timeScale = Mathf.Lerp(timeScale, 1f, elapsedTime / smoothTime);
            cameraController.SetFov(Mathf.Lerp(fov + 20, fov, elapsedTime / smoothTime));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public override void Select()
    {
        distanceCounter.SetActive(true);
        timerText.text = string.Empty;
    }

    public override void Deselect()
    {
        distanceCounter.SetActive(false);
    }
}
