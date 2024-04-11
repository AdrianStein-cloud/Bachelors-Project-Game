using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class Sandevistan : CooldownItem
{
    public float duration;
    public float timeScale;

    bool inUse = false;
    CameraController cameraController;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    public override void Primary() => TrySlowTime();

    private void TrySlowTime()
    {
        if (IsOffCooldown & !inUse)
        {
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
        inUse = true;
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
        StartCoroutine(Cooldown());
        inUse = false;
    }


    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
