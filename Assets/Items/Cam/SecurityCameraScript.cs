using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecurityCameraScript : MonoBehaviour
{
    float timePlaced = 0;

    public TextMeshProUGUI timePlacedText;
    public TextMeshProUGUI nameText;
    public string camName;

    new Camera camera;
    TabletGadget tablet;
    public LayerMask layerCol;

    public void Init(TabletGadget tablet)
    {
        this.tablet = tablet;
        camera = GetComponentInChildren<Camera>();
        tablet.battery.OnDead += RemoveCamera;
        tablet.onToggle += RemoveCameraIfFalse;

        nameText.text = camName;
        UnitySingleton<CameraManager>.Instance.cameraPositions.Add(camera, gameObject);
    }

    private void OnDestroy()
    {
        if (tablet != null)
        {
            tablet.battery.OnDead -= RemoveCamera;
            tablet.onToggle -= RemoveCameraIfFalse;
        }

        if (camera != null)
        {
            UnitySingleton<CameraManager>.Instance.activeCameras.Remove(camera);
            UnitySingleton<CameraManager>.Instance.cameraPositions.Remove(camera);
        }
    }

    private void Update()
    {
        timePlaced += Time.deltaTime;

        var actualMinutes = (int)timePlaced / 60;
        var minutes = actualMinutes % 60;
        var hours = actualMinutes / 60;
        var seconds = (int)timePlaced % 60;
        var miliseconds = (timePlaced - (int)timePlaced) * 1000;

        timePlacedText.text = string.Format("{0:00}.{1:00}.{2:00}", hours, minutes, seconds);
    }

    void RemoveCamera()
    {
        UnitySingleton<CameraManager>.Instance.activeCameras.Remove(camera);
    }

    void RemoveCameraIfFalse(bool value)
    {
        if(!value) RemoveCamera();
    }
}
