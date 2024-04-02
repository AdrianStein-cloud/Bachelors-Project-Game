using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensorController : Item
{
    public Action<GameObject> OnSensorBeep;

    [SerializeField] GameObject sensorPrefab;
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField] LayerMask placeLayer;
    [SerializeField] float placeDistance;
    [SerializeField] float laserDistance;

    [field: SerializeField] public int SensorCount { get; private set; }

    bool maximumPlaced => currentSensorCount == 0;

    List<GameObject> sensors;

    RaycastHit hit;
    GameObject ghostSensor;
    MeshRenderer sensorRenderer;
    bool isSelected;
    bool canPlace;
    int currentSensorCount;

    private void Awake()
    {
        ghostSensor = Instantiate(sensorPrefab);
        ghostSensor.SetActive(false);
        sensorRenderer = ghostSensor.GetComponentInChildren<MeshRenderer>();

        currentSensorCount = SensorCount;
        sensors = new();

        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.OnWaveOver += () =>
            {
                currentSensorCount = SensorCount;
                UpdateCounter();
            };
    }

    private void Start()
    {
        UpdateCounter();
    }

    private void Update()
    {
        if (!isSelected || maximumPlaced) return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, placeDistance, placeLayer))
        {
            ghostSensor.SetActive(true);
            ghostSensor.transform.position = hit.point;
            ghostSensor.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Rotate(ghostSensor.transform);

            canPlace = Physics.Raycast(hit.point, hit.normal, laserDistance, placeLayer);
            sensorRenderer.material = canPlace ? validMaterial : invalidMaterial;
        }
        else
        {
            canPlace = false;
            ghostSensor.SetActive(false);
        }
    }

    void Rotate(Transform sensor)
    {
        bool areParallel = Mathf.Approximately(Mathf.Abs(Vector3.Dot(Vector3.up, hit.normal)), 1f);
        Vector3 newForward = areParallel ? Vector3.right : Vector3.ProjectOnPlane(Vector3.up, hit.normal).normalized;
        sensor.rotation = Quaternion.LookRotation(newForward, hit.normal);
    }

    public override void Primary() => TryPlace();

    public override void Select()
    {
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
        canPlace = false;
        ghostSensor.SetActive(false);
    }

    private void TryPlace()
    {
        if (!canPlace || maximumPlaced) return;

        var instance = Instantiate(sensorPrefab, hit.point, Quaternion.identity);
        Rotate(instance.transform);
        instance.transform.SetParent(hit.transform);

        var sensor = instance.GetComponent<Sensor>();
        sensor.Init(OnSensorBeep);

        sensors.Add(instance);
        currentSensorCount--;

        UpdateCounter();

        if (maximumPlaced)
        {
            canPlace = false;
            ghostSensor.SetActive(false);
        }
    }

    private void UpdateCounter() => UnitySingleton<Inventory>.Instance.UpdateItemText(this, currentSensorCount.ToString());

    public void Upgrade(int count)
    {
        SensorCount += count;
        currentSensorCount = SensorCount;
        UpdateCounter();
    }
}
