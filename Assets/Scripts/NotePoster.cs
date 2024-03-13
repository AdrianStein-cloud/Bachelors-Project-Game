using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotePoster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notes;

    private void Start()
    {
        GameSettings.Instance.OnEventChanged += (value) =>
        {
            Debug.Log("Event: " + value);
            notes.text = value;
        };
    }
}
