using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject pauseScreen;

    private bool gamePaused = true;

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        resumeButton.onClick.AddListener(UnpauseGame);
        InputManager.Actions.Player.Pause.started += _ => PauseGame();
        InputManager.Actions.UI.Unpause.started += _ => UnpauseGame();
        UnpauseGame();
    }

    private void UnpauseGame()
    {
        if (!gamePaused) return;
        //Time.timeScale = 1f;
        gamePaused = false;
        pauseScreen.SetActive(false);
        InputManager.Actions.Player.Enable();
        InputManager.Actions.UI.Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PauseGame()
    {
        if (gamePaused) return;
        //Time.timeScale = 0f;
        gamePaused = true;
        pauseScreen.SetActive(true);
        InputManager.Actions.Player.Disable();
        InputManager.Actions.UI.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ExitGame()
    {
        //Time.timeScale = 1f;
        gamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void RestartGame()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
