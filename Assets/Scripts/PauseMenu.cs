using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button backSettingsButton;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_InputField sensInputField;


    private bool gamePaused = true;
    private CameraController player;

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        resumeButton.onClick.AddListener(UnpauseGame);
        settingsButton.onClick.AddListener(OpenSettings);
        backSettingsButton.onClick.AddListener(CloseSettings);
        InputManager.Actions.Player.Pause.started += _ => PauseGame();
        InputManager.Actions.UI.Unpause.started += _ => UnpauseGame();
        settingsMenu.SetActive(false);
        player = GameObject.FindObjectOfType<CameraController>();
        player.HorizontalSensitivity = 20f;
        player.VerticalSensitivity = 20f;
        sensSlider.value = 20f;
        sensSlider.onValueChanged.AddListener(SetSensitivity);
        sensInputField.text = "20";
        sensInputField.onValueChanged.AddListener(SetSensitivity);
        UnpauseGame();
    }

    private void SetSensitivity(float input)
    {
        sensInputField.text = input.ToString();
        player.HorizontalSensitivity = input;
        player.VerticalSensitivity = input;
    }

    private void SetSensitivity(string input)
    {
        float value = float.Parse(input);
        sensSlider.value = value;
        player.HorizontalSensitivity = value;
        player.VerticalSensitivity = value;
    }

    private void OpenSettings()
    {
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    private void CloseSettings()
    {
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        settingsMenu.SetActive(false);
    }

    private void UnpauseGame()
    {
        if (!gamePaused) return;
        //Time.timeScale = 1f;
        CloseSettings();
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
        CloseSettings();
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
