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
    private static bool isVisible = true;
    public Image _lock;
    private bool locked;

    private void Start()
    {
        isVisible = true;
        background = gameObject.transform.parent.GetComponent<Image>();
        background.color = new Color(1, 1, 1, 0);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        loadingScreen.SetActive(false);
        CheckIfLocked();
    }

    private void CheckIfLocked()
    {
        switch (difficulty)
        {
            case Difficulty.Normal:
                Lock(false);
                break;
            case Difficulty.Hard:
                Lock(false);
                break;
            case Difficulty.Nightmare:
                if(PlayerPrefs.GetInt("high_score_" + Difficulty.Hard) <= 5) Lock(true);
                else Lock(false);
                break;
            case Difficulty.Hell:
                if (PlayerPrefs.GetInt("high_score_" + Difficulty.Nightmare) <= 5) Lock(true);
                else Lock(false);
                break;
        }
    }

    private void Lock(bool locked)
    {
        this.locked = locked;

        if(_lock != null) _lock.gameObject.SetActive(locked);

        if (locked)
        {
            buttonText.color = new Color32(0x3D, 0x3D, 0x3D, 0xFF); //Grey
        }
        else
        {
            buttonText.color = new Color32(0x64, 0x00, 0x00, 0xFF); //Dark Red
        }
    }

    public void ChangeBackground()
    {
        if (locked) return;
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
        if (!isVisible)
        {
            gameObject.SetActive(false);
        }
    }

    public void Defocus()
    {
        if (locked) return;
        buttonText.color = new Color32(0x64, 0x00, 0x00, 0xFF);
    }

    public void StartGame()
    {
        isVisible = false;
        loadingScreen.SetActive(true);
        PlayerPrefs.SetInt("Difficulty", (int) difficulty);
        SceneManager.LoadScene("Procedural Generation");
    }
}
