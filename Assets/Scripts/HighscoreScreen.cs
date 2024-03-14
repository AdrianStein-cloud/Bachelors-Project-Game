using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;

    void Start()
    {
        highscoreText.text = PlayerPrefs.GetInt("high_score").ToString();
    }
}
