using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotePoster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notes;
    private string currentEvent;

    private void Start()
    {
        GameSettings.Instance.onEventChanged += () =>
        {
            currentEvent = GameSettings.Instance.Event;
            if (currentEvent == null)
            {
                notes.text = string.Empty;
            }
            else
            {
                notes.text = currentEvent;
            }
        };
    }
}
