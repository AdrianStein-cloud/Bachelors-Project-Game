using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceController : Item
{
    [SerializeField] Transform target;
    [SerializeField] float multiplier;

    GameObject distanceCounter;
    TextMeshProUGUI distanceText;

    private void Awake()
    {
        distanceCounter = FindObjectOfType<Canvas>().transform.Find("Distance Counter").gameObject;
        distanceText = distanceCounter.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        var exit = FindObjectOfType<DungeonExit>();
        if (target == null && exit != null) target = exit.transform;
    }

    private void Update()
    {
        if (!distanceCounter.activeSelf || target == null) return;

        var distance = Vector3.Distance(transform.position, target.position) * multiplier;
        distanceText.text = distance.ToString("F0") + " m";
    }

    public override void Select()
    {
        var exit = FindObjectOfType<DungeonExit>();
        if (exit != null) target = exit.transform;
        distanceCounter.SetActive(true);
    }

    public override void Deselect()
    {
        distanceCounter.SetActive(false);
    }
}
