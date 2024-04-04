using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

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
        distanceCounter = GameSettings.Instance.canvas.transform.Find("GadgetText").gameObject;
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
        float fov = 20;
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
        float currentFovIncrease = 0;
        while (elapsedTime < smoothTime)
        {
            Time.timeScale = Mathf.Lerp(startValue, timeScale, elapsedTime / smoothTime);
            float fovIncrement = Mathf.Lerp(0, fov, elapsedTime / smoothTime) - currentFovIncrease;
            cameraController.IncrementFov(fovIncrement);
            currentFovIncrease += fovIncrement;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOut(float fov)
    {
        Time.timeScale = 1f;
        //cameraController.SetFov(fov);

        float elapsedTime = 0f;
        float smoothTime = 1f;
        PostProcessingHandler.Instance.ResetBloom(smoothTime);
        PostProcessingHandler.Instance.SetChromaticAberration(smoothTime);
        PostProcessingHandler.Instance.ResetSaturation(smoothTime);
        float currentFovDecrease = 0;
        while (elapsedTime < smoothTime)
        {
            Time.timeScale = Mathf.Lerp(timeScale, 1f, elapsedTime / smoothTime);
            float fovIncrement = Mathf.Lerp(0, fov, elapsedTime / smoothTime) - currentFovDecrease;
            cameraController.IncrementFov(-fovIncrement);
            currentFovDecrease += fovIncrement;
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
