using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceController : Item
{
    [SerializeField] Transform target;
    [SerializeField] float multiplier;

    bool isSelected = false;

    GameObject distanceCounter;
    TextMeshProUGUI distanceText;

    private void Awake()
    {
        distanceCounter = GameSettings.Instance.canvas.transform.Find("Distance Counter").gameObject;
        distanceText = distanceCounter.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        var entrance = FindObjectOfType<DungeonEntrance>();
        if (entrance != null) 
            entrance.EnterDungeon += () =>
            {
                var exit = FindObjectOfType<DungeonExit>();
                if (exit != null) target = exit.transform;
            };
        var exit = FindObjectOfType<DungeonExit>();
        if (target == null && exit != null) target = exit.transform;
    }

    private void Update()
    {
        if (!isSelected || target == null) return;

        var distance = Vector3.Distance(transform.position, target.position) * multiplier;
        distanceText.text = distance.ToString("F0") + " m";
    }

    public override void Select()
    {
        var exit = FindObjectOfType<DungeonExit>();
        if (exit != null) target = exit.transform;
        distanceCounter.SetActive(true);
        distanceText.text = string.Empty;
        isSelected = true;
    }

    public override void Deselect()
    {
        distanceCounter.SetActive(false);
        isSelected = false;
    }
}
