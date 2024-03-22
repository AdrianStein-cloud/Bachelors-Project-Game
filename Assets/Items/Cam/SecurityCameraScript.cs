using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecurityCameraScript : MonoBehaviour
{
    public bool colliding;
    float timePlaced = 0;

    public TextMeshProUGUI timePlacedText;

    private void Update()
    {
        timePlaced += Time.deltaTime;

        var actualMinutes = (int)timePlaced / 60;
        var minutes = actualMinutes % 60;
        var hours = actualMinutes / 60;
        var seconds = (int)timePlaced % 60;
        var miliseconds = (timePlaced - (int)timePlaced) * 1000;

        timePlacedText.text = string.Format("{0:00}.{1:00}.{2:00}", hours, minutes, seconds);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Room")) colliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Room")) colliding = false;
    }
}
