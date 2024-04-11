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
        Refresh();
        UnitySingleton<GameManager>.Instance.OnDungeonGenerated += _ =>
        {
            Refresh();
        };
    }

    private void Refresh()
    {
        highscoreText.text = PlayerPrefs.GetInt("high_score_" + GameSettings.Instance.DifficultyConfig.difficulty).ToString();
        difficultyText.text = GameSettings.Instance.DifficultyConfig.difficulty.ToString();
    }
}
