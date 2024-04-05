using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public readonly List<Camera> activeCameras = new List<Camera>();
    public readonly Dictionary<Camera, GameObject> cameraPositions = new Dictionary<Camera, GameObject>();


    private void Awake()
    {
        UnitySingleton<CameraManager>.BecomeSingleton(this);
    }
}
