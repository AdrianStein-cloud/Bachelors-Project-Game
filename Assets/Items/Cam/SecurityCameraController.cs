using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class SecurityCameraController : Item
{
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField] LayerMask placeLayer;
    [SerializeField] float placeDistance;
    [SerializeField] RenderTexture tabletTexture;
    public TabletGadget tablet;
    public float batteryDrain;

    [field: SerializeField] public int MaxCamCount { get; private set; }

    bool maximumPlaced => currentCamCount == 0;

    public List<GameObject> cameras;
    int currentCameraIndex = 0;

    RaycastHit hit;
    GameObject ghostCamera;
    SecurityCameraScript ghostScript;
    bool isSelected;
    bool canPlace;
    int currentCamCount;

    List<MeshRenderer> renderers;

    private void Awake()
    {
        tablet = GameObject.FindGameObjectWithTag("Tablet").GetComponent<TabletGadget>();

        ghostCamera = Instantiate(cameraPrefab);
        ghostCamera.SetActive(false);
        ghostScript = ghostCamera.GetComponent<SecurityCameraScript>();
        //cameraRenderer = ghostCamera.GetComponentInChildren<MeshRenderer>();
        renderers = new List<MeshRenderer>();
        foreach (var renderer in ghostCamera.GetComponentsInChildren<MeshRenderer>())
        {
            renderers.Add(renderer);
        }
        currentCamCount = MaxCamCount;

        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.OnWaveOver += () =>
            {
                foreach (var cam in cameras)
                {
                    if (cam != null) Destroy(cam.gameObject);
                }

                cameras = new List<GameObject>();

                currentCamCount = MaxCamCount;
                currentCameraIndex = 0;
                UpdateCounter();
            };
    }

    private void Start()
    {
        UpdateCounter();
    }

    private void Update()
    {
        if (!isSelected) return;

        if ((!tablet.equipped) && !maximumPlaced && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, placeDistance, placeLayer))
        {
            ghostCamera.SetActive(true);
            ghostCamera.transform.position = hit.point;
            ghostCamera.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Rotate(ghostCamera.transform);


            UpdateMaterials(validMaterial);
            canPlace = true;

        }
        else
        {
            canPlace = false;
            ghostCamera.SetActive(false);
        }
    }

    void UpdateMaterials(Material mat)
    {
        foreach (var renderer in renderers)
        {
            renderer.material = mat;
        }
    }

    public override void Primary()
    {
        if (!tablet.equipped) TryPlace();
        else
        {
            //switch cameras
            if (cameras.Count <= 0) return;

            ToggleCamera(cameras[currentCameraIndex], false);

            currentCameraIndex = ((currentCameraIndex + 1) % cameras.Count);

            ToggleCamera(cameras[currentCameraIndex], true);
        }
    }

    void ToggleCamera(GameObject cam, bool enable)
    {
        if (!tablet.battery.Dead) tablet.textureRenderer.SetActive(true);
        var camera = cam.GetComponentInChildren<Camera>();
        camera.targetTexture = enable ? tabletTexture : null;
        if (enable & !tablet.battery.Dead)
        {
            UnitySingleton<CameraManager>.Instance.activeCameras.Add(camera);
        }
        if (!enable)
        {
            UnitySingleton<CameraManager>.Instance.activeCameras.Remove(camera);
        }
    }

    public override void Secondary()
    {
        if (tablet.Toggle())
        {
            if (tablet.battery.Dead) return;

            if (cameras.Count > 0) tablet.battery.ToggleBattery(batteryDrain);

            if (tablet.equipped)
            {
                DefaultScreen();
            }
        }
    }

    public override void Select()
    {
        tablet.holdingTabletGadget = true;
        isSelected = true;

        tablet.battery.Select();
        if (tablet.battery.Dead) return;
        if (tablet.equipped)
        {
            DefaultScreen();
            if (cameras.Count > 0) tablet.battery.ToggleBattery(batteryDrain);
        }
    }

    void DefaultScreen()
    {
        if (cameras.Count > 0)
        {
            tablet.textureRenderer.SetActive(true);
            ToggleCamera(cameras[currentCameraIndex], true);
        }
        else
        {
            tablet.textureRenderer.SetActive(false);
        }
    }

    public override void Deselect()
    {
        tablet.holdingTabletGadget = false;
        isSelected = false;
        canPlace = false;
        ghostCamera.SetActive(false);
        if (cameras.Count > 0) ToggleCamera(cameras[currentCameraIndex], false);
        tablet.textureRenderer.SetActive(false);
        if (tablet.battery.On) tablet.battery.ToggleBattery(0);
        tablet.battery.Deselect();
        tablet.SwitchGadget();
    }

    private void TryPlace()
    {
        if (!canPlace || maximumPlaced) return;

        var instance = Instantiate(cameraPrefab, hit.point, Quaternion.identity);
        var camScript = instance.GetComponent<SecurityCameraScript>();


        Rotate(instance.transform);
        instance.transform.SetParent(hit.transform);

        cameras.Add(instance);
        camScript.camName = "CAMERA " + cameras.Count;

        camScript.Init(tablet);

        currentCamCount--;
        UpdateCounter();

        if (maximumPlaced)
        {
            canPlace = false;
            ghostCamera.SetActive(false);
        }
    }

    void Rotate(Transform sensor)
    {
        bool areParallel = Mathf.Approximately(Mathf.Abs(Vector3.Dot(Vector3.up, hit.normal)), 1f);
        Vector3 newForward = areParallel ? Vector3.up : Vector3.ProjectOnPlane(Vector3.up, hit.normal).normalized;
        sensor.rotation = Quaternion.LookRotation(hit.normal, newForward);
    }

    private void UpdateCounter() => UnitySingleton<Inventory>.Instance.UpdateItemText(this, currentCamCount.ToString());
}
