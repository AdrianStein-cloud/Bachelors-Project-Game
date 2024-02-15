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
    Action leave;

    int collectedObjectives = 0;

    private void Awake()
    {
        GameObject.Find("DungeonExit").GetComponent<DungeonExit>().LeaveDungeon = AttemptLeave;
    }

    public void Init(int maxObjectives, int leaveThreshold, Image objectiveFill, Action leave)
    {
        this.maxObjectives = maxObjectives;
        this.leaveThreshold = leaveThreshold;
        this.objectiveFill = objectiveFill;
        this.leave = leave;
    }

    public void ObjetiveCollected()
    {
        collectedObjectives++;
        objectiveFill.fillAmount = collectedObjectives / (float)maxObjectives;
        if(collectedObjectives >= leaveThreshold)
        {
            GameObject.Find("DungeonExit").GetComponent<DungeonExit>().CanLeaveDungeon = true;
        }
    }

    public bool AttemptLeave()
    {
        if (collectedObjectives >= leaveThreshold)
        {
            leave();
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        if(objectiveFill) objectiveFill.fillAmount = 0;
    }
}
