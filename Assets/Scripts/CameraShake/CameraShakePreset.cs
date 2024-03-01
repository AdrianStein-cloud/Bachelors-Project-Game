using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Camerashake Preset", menuName = "Camera Shake")]
public class CameraShakePreset : ScriptableObject
{
    [Header("RotationShake")]
    public float rotationSteps;
    public float rotationShakeTime;
    public float rotationAmount;
    public float rotationAngle;
    public bool rotationShake;

    [Header("ScaleShake")]
    public float scaleShakeTime;
    public int zoomSteps;
    public float zoomAmount;
    public bool scaleShake;

    [Header("MovementShake")]
    public float moveRandom;
    public float moveShakeTime;
    public int moveSteps;
    public bool movementShake;
}
