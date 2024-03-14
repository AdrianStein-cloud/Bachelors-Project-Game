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

    [field: SerializeField] public int MaxCamCount { get; private set; }

    bool maximumPlaced => currentCamCount == 0;

    public List<GameObject> cameras;
    int currentCameraIndex = 0;

    RaycastHit hit;
    GameObject ghostCamera;
    bool isSelected;
    bool canPlace;
    int currentCamCount;

    List<MeshRenderer> renderers;

    private void Awake()
    {
        tablet = GameObject.FindGameObjectWithTag("Tablet").GetComponent<TabletGadget>();

        ghostCamera = Instantiate(cameraPrefab);
        ghostCamera.SetActive(false);
        //cameraRenderer = ghostCamera.GetComponentInChildren<MeshRenderer>();
        renderers = new List<MeshRenderer>();
        foreach(var renderer in ghostCamera.GetComponentsInChildren<MeshRenderer>())
        {
            renderers.Add(renderer);
        }
        currentCamCount = MaxCamCount;
    }

    private void Update()
    {
        if (!isSelected) return;

        if ((!tablet.tabletEquipped) && !maximumPlaced && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, placeDistance, placeLayer))
        {
            ghostCamera.SetActive(true);
            ghostCamera.transform.position = hit.point;
            ghostCamera.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Rotate(ghostCamera.transform);

            var ghostScript = ghostCamera.GetComponent<SecurityCameraScript>();
            if (ghostScript.colliding)
            {
                UpdateMaterials(invalidMaterial);
                canPlace = false;
            }
            else
            {
                UpdateMaterials(validMaterial);
                canPlace = true;
            }
        }
        else
        {
            canPlace = false;
            ghostCamera.SetActive(false);
        }
    }

    void UpdateMaterials(Material mat)
    {
        foreach(var renderer in renderers)
        {
            renderer.material = mat;
        }
    }

    public override void Primary()
    {
        if (!tablet.tabletEquipped) TryPlace();
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
        tablet.textureRenderer.SetActive(true);
        var camera = cam.GetComponentInChildren<Camera>();
        camera.targetTexture = enable ? tabletTexture : null;
    }

    public override void Secondary()
    {
        tablet.Toggle();
        if (tablet.tabletEquipped)
        {
            if (cameras.Count > 0)
            {
                tablet.textureRenderer.SetActive(true);
                ToggleCamera(cameras[currentCameraIndex], true);
            }
            else tablet.textureRenderer.SetActive(false);
        }
    }

    public override void Select()
    {
        tablet.holdingTabletGadget = true;
        isSelected = true;
        if (cameras.Count > 0)
        {
            tablet.textureRenderer.SetActive(true);
            ToggleCamera(cameras[currentCameraIndex], true);
        }
        else tablet.textureRenderer.SetActive(false);
    }

    public override void Deselect()
    {
        tablet.holdingTabletGadget = false;
        isSelected = false;
        canPlace = false;
        ghostCamera.SetActive(false);
        if (cameras.Count > 0) ToggleCamera(cameras[currentCameraIndex], false);
        tablet.textureRenderer.SetActive(false);
        StartCoroutine(tablet.SwitchGadget());
    }

    private void TryPlace()
    {
        if (!canPlace || maximumPlaced) return;

        var instance = Instantiate(cameraPrefab, hit.point, Quaternion.identity);

        Rotate(instance.transform);

        cameras.Add(instance);
        currentCamCount--;

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
}
