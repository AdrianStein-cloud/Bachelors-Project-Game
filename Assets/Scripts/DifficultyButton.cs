using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private Image background;
    private TextMeshProUGUI buttonText;
    public Difficulty difficulty;
    public GameObject loadingScreen;
    private static bool isEnabled = true;

    private void Start()
    {
        isEnabled = true;
        background = gameObject.transform.parent.GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        loadingScreen.SetActive(false);
    }

    public void ChangeBackground()
    {
        buttonText.color = new Color32(0xFF, 0x59, 0x67, 0xFF);
        switch (difficulty)
        {
            case Difficulty.Normal:
                background.color = new Color(1, 1, 1, 0);
                break;
            case Difficulty.Hard:
                background.color = new Color(1, 1, 1, 1f/4f);
                break;
            case Difficulty.Nightmare:
                background.color = new Color(1, 1, 1, 1f/2f);
                break;
            case Difficulty.Hell:
                background.color = new Color(1, 1, 1, 1);
                break;
        }
    }

    private void Update()
    {
        if (!isEnabled)
        {
            gameObject.SetActive(false);
        }
    }

    public void Defocus()
    {
        buttonText.color = new Color32(0x64, 0x00, 0x00, 0xFF);
    }

    public void StartGame()
    {
        isEnabled = false;
        loadingScreen.SetActive(true);
        PlayerPrefs.SetInt("Difficulty", (int) difficulty);
        SceneManager.LoadScene("Procedural Generation");
    }
}
