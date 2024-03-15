using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class PostProcessingHandler : MonoBehaviour
{
    public static PostProcessingHandler Instance;

    Volume volume;

    Vignette vignette;
    DepthOfField dof;
    ColorAdjustments colorAdjustments;
    LensDistortion lensDistortion;
    ChromaticAberration chromaticAberration;
    WhiteBalance whiteBalance;
    Bloom bloom;
    MotionBlur motionBlur;

    Color defaultColorFilter;
    float defaultExposure;
    float defaultSaturation;

    float defaultVignette;
    float defaultBloom;
    public float VignetteValue { get; set; }
    public Color ColorFilter { get; set; }

    private void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out whiteBalance);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out motionBlur);

        defaultColorFilter = ColorFilter = colorAdjustments.colorFilter.value;
        defaultExposure = colorAdjustments.postExposure.value;
        defaultVignette = VignetteValue = vignette.intensity.value;
        defaultBloom = bloom.intensity.value;
        defaultSaturation = colorAdjustments.saturation.value;
    }

    public void SetBloom(float smoothTime, float value = 0)
    {
        Value(bloom.intensity, smoothTime, value);
    }

    public void ResetBloom(float smoothTime)
    {
        Value(bloom.intensity, smoothTime, defaultBloom);
    }

    void Value(FloatParameter parameter, float smoothTime, float value)
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            float elapsedTime = 0;
            var startValue = parameter.value;
            while (elapsedTime < smoothTime)
            {
                parameter.value = Mathf.Lerp(startValue, value, elapsedTime / smoothTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    void Value2(Vector2Parameter parameter, float smoothTime, Vector2 value)
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            float elapsedTime = 0;
            var startValue = parameter.value;
            while (elapsedTime < smoothTime)
            {
                parameter.value = Vector2.Lerp(startValue, value, elapsedTime / smoothTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    public void SetWhiteBalance(float value = 0)
    {
        whiteBalance.temperature.value = value;
    }

    public void SetDOF(bool blur)
    {
        dof.active = blur;
    }

    public void SetLensDistortion(float smoothTime, float value = 0)
    {
        Value(lensDistortion.intensity, smoothTime, value);
    }

    public void SetChromaticAberration(float smoothTime, float value = 0)
    {
        Value(chromaticAberration.intensity, smoothTime, value);
    }
    public void SetMotionBlur(float smoothTime, float value = 0)
    {
        Value(motionBlur.intensity, smoothTime, value);
    }

    public void ResetColorFilter()
    {
        ColorFilter = defaultColorFilter;
    }

    public void SetColorFilter(Color colorFilter = default)
    {
        colorAdjustments.colorFilter.value = colorFilter == default ? ColorFilter : colorFilter;
    }

    public void ResetSaturation(float smoothTime)
    {
        Value(colorAdjustments.saturation, smoothTime, defaultSaturation);
    }

    public void SetSaturation(float smoothTime, float value)
    {
        Value(colorAdjustments.saturation, smoothTime, value);
    }

    public void ResetPostExposure(float smoothTime)
    {
        Value(colorAdjustments.postExposure, smoothTime, defaultExposure);
    }

    public void SetPostExposure(float smoothTime, float value = 0)
    {
        Value(colorAdjustments.postExposure, smoothTime, value);
    }

    public void ResetVignette(float smoothTime = 0)
    {
        VignetteValue = defaultVignette;
        if (smoothTime > 0) SetVignette(smoothTime);
    }

    public void SetVignette(float smoothTime, Vector2 offset = default, float offsetTime = 3)
    {
        SetVignette(VignetteValue, smoothTime, offset, offsetTime);
    }

    public void SetVignette(float value, float smoothTime, Vector2 offset = default, float offsetTime = 3)
    {
        offset = offset == Vector2.zero ? new Vector2(0.5f, 0.5f) : offset;

        Value2(vignette.center, offsetTime, offset);
        Value(vignette.intensity, smoothTime, value);
    }
}
