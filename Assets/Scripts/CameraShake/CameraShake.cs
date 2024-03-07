using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    public CameraShakePreset Defaultpreset;

    Camera cam;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Shake(Defaultpreset);
        }
    }*/

    public void Shake(CameraShakePreset preset)
    {
        if (preset.rotationShake) RotationShake(preset);
        if (preset.scaleShake) ScaleShake(preset);
        if (preset.movementShake) MovementShake(preset);
    }

    void RotationShake(CameraShakePreset preset)
    {
        StartCoroutine(RotShake(preset));
    }

    IEnumerator RotShake(CameraShakePreset preset)
    {
        Quaternion startRot = cam.transform.localRotation;
        for (int i = 0; i < preset.rotationAmount; i++)
        {
            for (int j = 0; j < preset.rotationSteps / preset.rotationAmount; j++)
            {
                cam.transform.localRotation = Quaternion.Lerp(startRot, 
                    Quaternion.Euler(cam.transform.localRotation.x, cam.transform.localRotation.y, preset.rotationAngle * (i % 2 > 0 ? -1 : 1)), 0.1f);

                yield return new WaitForSeconds(preset.rotationShakeTime / preset.rotationSteps / preset.rotationAmount);
            }
        }
        cam.transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 0);
    }

    void ScaleShake(CameraShakePreset preset)
    {
        StartCoroutine(ScaleShakeNum(preset));
    }

    IEnumerator ScaleShakeNum(CameraShakePreset preset)
    {
        float startFov = cam.fieldOfView;
        
        for (int i = 0; i < preset.zoomAmount; i++)
        {
            for (int j = preset.zoomSteps; j > 0; j--)
            {
                float fov = Random.Range(-preset.zoomAmount, preset.zoomAmount) * (j / (float)preset.zoomSteps);
                cam.fieldOfView = startFov + fov;

                yield return new WaitForSeconds((preset.scaleShakeTime / (float)preset.zoomSteps) / preset.zoomAmount);
            }
        }
        cam.fieldOfView = startFov;
    }

    void MovementShake(CameraShakePreset preset)
    {
        StartCoroutine(MoveShake(preset));
    }

    IEnumerator MoveShake(CameraShakePreset preset)
    {
        Vector3 startPos = cam.transform.localPosition;

        float xpos, ypos;

        for (int i = preset.moveSteps; i > 0; i--)
        {
            xpos = Random.Range(-preset.moveRandom, preset.moveRandom) * (i / (float)preset.moveSteps);
            ypos = Random.Range(-preset.moveRandom, preset.moveRandom) * (i / (float)preset.moveSteps);

            cam.transform.localPosition = new Vector3(startPos.x + xpos, startPos.y + ypos, cam.transform.localPosition.z);

            yield return new WaitForSeconds(preset.moveShakeTime / (float)preset.moveSteps);
        }

        cam.transform.localPosition = startPos;
    }

    enum Rotation { left, right }
}

