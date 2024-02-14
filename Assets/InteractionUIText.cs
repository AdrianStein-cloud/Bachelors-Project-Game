using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUIText : MonoBehaviour
{
    public static InteractionUIText Instance;

    TextMeshProUGUI textField;

    private void Awake()
    {
        Instance = this;
        textField = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string value)
    {
        textField.text = value;
    }
}
