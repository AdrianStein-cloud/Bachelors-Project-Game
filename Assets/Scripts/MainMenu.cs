using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button exitButton;
    [SerializeField] GameObject difficultyMenu;
    [SerializeField] GameObject credits;
    [SerializeField] bool alwaysNightmare = false;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
        difficultyMenu.SetActive(false);
        HideCredits();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    public void LoadDifficulties()
    {
        if (!alwaysNightmare) difficultyMenu.SetActive(true);
        else difficultyMenu.GetComponentInChildren<DifficultyButton>().StartGame(Difficulty.Nightmare);
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
    }
}
