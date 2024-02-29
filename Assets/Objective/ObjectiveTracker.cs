using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour
{
    int maxObjectives;
    int leaveThreshold;
    Image objectiveFill;
    Action<int> leave;

    int collectedObjectives = 0;
    DungeonExit exit;

    private void Awake()
    {
        exit = FindObjectOfType<DungeonExit>();
        exit.LeaveDungeon = AttemptLeave;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { leave(5); }
    }

    public void Init(int maxObjectives, int leaveThreshold, Image objectiveFill, Action<int> leave)
    {
        this.maxObjectives = maxObjectives;
        this.leaveThreshold = leaveThreshold;
        this.objectiveFill = objectiveFill;
        objectiveFill.color = new Color(1, 0, 0, 1);
        this.leave = leave;
    }

    public void ObjetiveCollected()
    {
        collectedObjectives++;
        float fill = collectedObjectives / (float)maxObjectives;
        objectiveFill.fillAmount = fill;
        objectiveFill.color = new Color(1 - fill, fill, 0, 1);
        if (collectedObjectives >= leaveThreshold)
        {
            exit.CanLeaveDungeon = true;
        }
    }

    public bool AttemptLeave()
    {
        if (collectedObjectives >= leaveThreshold)
        {
            int upgradesAmount = 2 * (collectedObjectives - leaveThreshold) / (maxObjectives - leaveThreshold) + 1;
            leave(upgradesAmount);
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        if(objectiveFill) objectiveFill.fillAmount = 0;
    }
}
