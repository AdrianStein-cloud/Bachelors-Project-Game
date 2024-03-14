using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotePoster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notes;

    private void Awake()
    {
        GameSettings.Instance.OnEventChanged += (value) =>
        {
            notes.text = value;
        };
    }
}
