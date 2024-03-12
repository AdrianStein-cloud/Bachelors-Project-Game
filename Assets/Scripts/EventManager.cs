using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    List<Action> events;

    public EventManager()
    {
        events = new List<Action>
        {
            PowerOutage,
            Flooded,
            Foggy,
            NoEvent
        };
    }

    public void SpawnRandomEvent(System.Random random)
    {
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
        //Not Implemented
        NoEvent();
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
}
