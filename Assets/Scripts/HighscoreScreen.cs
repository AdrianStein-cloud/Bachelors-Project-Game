using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI difficultyText;

    void Start()
    {
        highscoreText.text = PlayerPrefs.GetInt("high_score_" + GameSettings.Instance.DifficultyConfig.difficulty).ToString();
        difficultyText.text = GameSettings.Instance.DifficultyConfig.difficulty.ToString();
    }
}
