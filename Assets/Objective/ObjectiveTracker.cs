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
    Action heartCollected;

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

    public void Init(int maxObjectives, int leaveThreshold, Image objectiveFill, Action heartCollected, Action<int> leave)
    {
        this.maxObjectives = maxObjectives;
        this.leaveThreshold = leaveThreshold;
        this.objectiveFill = objectiveFill;
        objectiveFill.color = new Color(1, 0, 0, 1);
        this.leave = leave;
        this.heartCollected = heartCollected;
    }

    public void ObjetiveCollected()
    {
        collectedObjectives++;
        float fill = collectedObjectives / (float)leaveThreshold;
        objectiveFill.fillAmount = fill;
        objectiveFill.color = new Color(1 - fill, fill, 0, 1);
        heartCollected();
        if (collectedObjectives >= leaveThreshold)
        {
            exit.CanLeaveDungeon = true;
        }
    }

    public bool AttemptLeave()
    {
        if (collectedObjectives >= leaveThreshold)
        {
            int upgradesAmount = 2 * (Mathf.Min(collectedObjectives, maxObjectives) - leaveThreshold) / (maxObjectives - leaveThreshold) + 1;
            leave(3);
            GameSettings.Instance.PlayerInDungeon = false;
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        if(objectiveFill) objectiveFill.fillAmount = 0;
    }
}
