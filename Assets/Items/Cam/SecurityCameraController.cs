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
    [SerializeField] float itemToggleDelay;
    [SerializeField] RenderTexture tabletTexture;
    public GameObject tabletPrefab;
    public GameObject tablet;

    [field: SerializeField] public int MaxCamCount { get; private set; }

    bool maximumPlaced => currentCamCount == 0;

    public List<GameObject> cameras;
    int currentCameraIndex = 0;

    RaycastHit hit;
    GameObject ghostCamera;
    bool isSelected;
    bool canPlace;
    int currentCamCount;

    bool tabletEquipped;
    Animator tabletAnim;
    float lastTimeUsed;

    List<MeshRenderer> renderers;

    private void Awake()
    {
        tablet = Instantiate(tabletPrefab, Camera.main.transform);

        ghostCamera = Instantiate(cameraPrefab);
        ghostCamera.SetActive(false);
        //cameraRenderer = ghostCamera.GetComponentInChildren<MeshRenderer>();
        renderers = new List<MeshRenderer>();
        foreach(var renderer in ghostCamera.GetComponentsInChildren<MeshRenderer>())
        {
            renderers.Add(renderer);
        }
        currentCamCount = MaxCamCount;
        tabletAnim = tablet.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isSelected) return;

        if ((!tabletEquipped) && !maximumPlaced && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, placeDistance, placeLayer))
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
        if (!tabletEquipped) TryPlace();
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
        var camera = cam.GetComponentInChildren<Camera>();
        camera.targetTexture = enable ? tabletTexture : null;
    }

    public override void Secondary()
    {
        if (lastTimeUsed + itemToggleDelay <= Time.time)
        {
            lastTimeUsed = Time.time;
            tabletEquipped = !tabletEquipped;
            tabletAnim.SetTrigger("Toggle");
        }
    }

    public override void Select()
    {
        isSelected = true;
        tablet.SetActive(true);
    }

    public override void Deselect()
    {
        isSelected = false;
        canPlace = false;
        ghostCamera.SetActive(false);
        tablet.SetActive(false);
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
