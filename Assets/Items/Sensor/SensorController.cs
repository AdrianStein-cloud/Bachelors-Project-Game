using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : Item
{
    [SerializeField] GameObject sensorPrefab;
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField] LayerMask placeLayer;
    [SerializeField] float placeDistance;
    [SerializeField] float laserDistance;

    [field: SerializeField] public int SensorCount { get; private set; }

    bool maximumPlaced => currentSensorCount >= SensorCount;

    List<GameObject> sensors;

    RaycastHit hit;
    GameObject ghostSensor;
    MeshRenderer sensorRenderer;
    bool isSelected;
    bool canPlace;
    int currentSensorCount;

    private void Start()
    {
        ghostSensor = Instantiate(sensorPrefab);
        ghostSensor.SetActive(false);
        sensorRenderer = ghostSensor.GetComponentInChildren<MeshRenderer>();
        sensors = new();
    }

    private void Update()
    {
        if (!isSelected || maximumPlaced) return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, placeDistance, placeLayer))
        {
            ghostSensor.SetActive(true);
            ghostSensor.transform.position = hit.point;
            ghostSensor.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (Physics.Raycast(hit.point, hit.normal, laserDistance, placeLayer))
            {
                sensorRenderer.material = validMaterial;
                canPlace = true;
            }
            else
            {
                sensorRenderer.material = invalidMaterial;
                canPlace = false;
            }
        }
        else
        {
            canPlace = false;
            ghostSensor.SetActive(false);
        }
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
        instance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        var sensor = instance.GetComponent<Sensor>();
        sensor.Init();

        sensors.Add(instance);
        currentSensorCount++;

        if (maximumPlaced)
        {
            canPlace = false;
            ghostSensor.SetActive(false);
        }
    }
}