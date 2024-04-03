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

    private void Start()
    {
        background = gameObject.transform.parent.GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ChangeBackground()
    {
        buttonText.color = new Color32(0xFF, 0x59, 0x67, 0xFF);
        switch (difficulty)
        {
            case Difficulty.Easy:
                background.color = new Color(1, 1, 1, 0);
                break;
            case Difficulty.Normal:
                background.color = new Color(1, 1, 1, 1f/4f);
                break;
            case Difficulty.Hard:
                background.color = new Color(1, 1, 1, 1f/2f);
                break;
            case Difficulty.Insane:
                background.color = new Color(1, 1, 1, 1);
                break;
        }
    }

    public void Defocus()
    {
        buttonText.color = new Color32(0x64, 0x00, 0x00, 0xFF);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("Difficulty", (int) difficulty);
        SceneManager.LoadScene("Procedural Generation");
    }
}
