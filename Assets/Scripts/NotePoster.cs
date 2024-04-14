using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotePoster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notes;

    private void Start()
    {
        GameSettings.Instance.OnEventChanged += (value) =>
        {
            notes.text = value;
            if(value == null)
            {
                notes.text = "Unknown.";
            }
        };
    }
}
