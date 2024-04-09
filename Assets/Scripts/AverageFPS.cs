using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AverageFPS : MonoBehaviour
{
    private TextMeshProUGUI text;

    float avg = 0f;

    void Start()
    {
        #if !UNITY_EDITOR
        this.gameObject.SetActive(false);
        #endif
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        avg += ((Time.deltaTime / Time.timeScale) - avg) * 0.03f;
        text.text = ((int) (1f / avg)).ToString();
    }
}
