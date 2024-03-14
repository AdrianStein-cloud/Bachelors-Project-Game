using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    List<Action> events;
    GameObject flood;

    public EventManager(GameObject flood)
    {
        this.flood = flood;
        flood.SetActive(false);

        events = new List<Action>
        {
            Flooded,
            PowerOutage,
            Foggy,
            NoEvent
        };
    }

    public void SpawnRandomEvent(System.Random random)
    {
        flood.gameObject.SetActive(false);
        events[random.Next(events.Count)].Invoke();
    }

    private void PowerOutage()
    {
        if(GameSettings.Instance.Wave > 2)
        {
            GameSettings.Instance.Event = "Power Outage!";
            GameSettings.Instance.LightFailPercentage = 100;
        }
        else
        {
            NoEvent();
        }
    }

    private void Flooded()
    {
        if (GameSettings.Instance.Wave > 0)
        {
            flood.SetActive(true);
            GameSettings.Instance.Event = "Flooded!";
        }
        else
        {
            NoEvent();
        }
    }

    private void Foggy()
    {
        //Not Implemented
        NoEvent();
    }

    private void NoEvent()
    {
        GameSettings.Instance.Event = null;
    }

    public void SetSizeOfDungeon(Vector3 size)
    {
        if (flood.activeInHierarchy)
        {
            flood.transform.localScale = new Vector3(size.x/10, 1, size.z/10);
        }
    }

    public void SetCenterOfDungeon(Vector3 center)
    {
        if (flood.activeInHierarchy)
        {
            flood.transform.position = new Vector3(center.x, 3, center.z);
        }
    }
}
