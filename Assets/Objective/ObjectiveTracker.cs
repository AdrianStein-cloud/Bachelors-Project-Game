using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour
{
    public int MaxObjectives { get; set; }
    public int LeaveThreshold { get; set; }
    public Image ObjectiveFill { get; set; }

    int collectedObjectives = 0;


    public void ObjetiveCollected()
    {
        collectedObjectives++;
        ObjectiveFill.fillAmount = collectedObjectives / (float)MaxObjectives;
    }

    private void OnDestroy()
    {
        if(ObjectiveFill) ObjectiveFill.fillAmount = 0;
    }
}
