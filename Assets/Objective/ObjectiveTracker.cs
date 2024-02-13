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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) AttemptLeave();
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
    }

    public void AttemptLeave()
    {
        if (collectedObjectives >= leaveThreshold)
        {
            leave();
        }
        else
        {
            Debug.LogWarning("Should display: \"Can't leave\"");
        }
    }

    private void OnDestroy()
    {
        if(objectiveFill) objectiveFill.fillAmount = 0;
    }
}