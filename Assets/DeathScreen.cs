using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public float delay;
    [SerializeField] private Button restartButton;
    private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI youDiedText;
    [SerializeField] private TextMeshProUGUI newDifficulty;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager.ReloadInputs();
        restartButton.onClick.AddListener(RestartGame);
        restartText = restartButton.GetComponentInChildren<TextMeshProUGUI>();
        restartButton.gameObject.SetActive(false);
        roundText.text = "Round " + PlayerPrefs.GetInt("player_score_" + GameSettings.Instance.DifficultyConfig.difficulty);
        roundText.gameObject.SetActive(false);
        highscoreText.gameObject.SetActive(false);
        newDifficulty.gameObject.SetActive(false);
        StartCoroutine(Restart());
    }

    private void RestartGame()
    {
        restartButton.enabled = false;
        restartText.text = "Loading...";
        SceneManager.LoadScene("Procedural Generation");
    }

    IEnumerator FadeIn(TextMeshProUGUI text)
    {
        Color color = text.color;
        for (int i = 0; i <= 100; i++)
        {
            color.a = (float) i/100;
            text.color = color;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Restart()
    {
        StartCoroutine(FadeIn(youDiedText));
        yield return new WaitForSeconds(delay);

        int score = PlayerPrefs.GetInt("player_score_" + GameSettings.Instance.DifficultyConfig.difficulty);
        int currentHighscore = PlayerPrefs.GetInt("high_score_" + GameSettings.Instance.DifficultyConfig.difficulty);

        if (score > currentHighscore)
        {
            PlayerPrefs.SetInt("high_score_" + GameSettings.Instance.DifficultyConfig.difficulty, score);
            highscoreText.gameObject.SetActive(true);

            switch (GameSettings.Instance.DifficultyConfig.difficulty)
            {
                case Difficulty.Hard:
                    if(currentHighscore <= 4 && score > 4)
                    {
                        newDifficulty.gameObject.SetActive(true);
                        newDifficulty.text = "Nightmare Unlocked!";
                        StartCoroutine(FadeIn(newDifficulty));
                    }
                    break;
                case Difficulty.Nightmare:
                    if (currentHighscore <= 5 && score > 5)
                    {
                        newDifficulty.gameObject.SetActive(true);
                        newDifficulty.text = "Hell Unlocked!";
                        StartCoroutine(FadeIn(newDifficulty));
                    }
                    break;
            }

            StartCoroutine(FadeIn(highscoreText));
        }

        roundText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        StartCoroutine(FadeIn(roundText));
        StartCoroutine(FadeIn(restartText));
    }

    private void CheckIfDifficultyUnlocked()
    {

    }
}
