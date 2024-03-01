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
    [SerializeField] Toggle crosshairToggle;
    [SerializeField] GameObject crosshair;


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
        var value = PlayerPrefs.GetFloat("Sensitivity");
        if (value == 0)
        {
            value = 20f;
            PlayerPrefs.SetFloat("Sensitivity", value);
        }
        player.HorizontalSensitivity = value;
        player.VerticalSensitivity = value;
        sensSlider.value = value;
        sensSlider.onValueChanged.AddListener(SetSensitivity);
        sensInputField.text = value.ToString();
        sensInputField.onValueChanged.AddListener(SetSensitivity);
        crosshair.SetActive(PlayerPrefs.GetInt("Crosshair") == 1);
        crosshairToggle.onValueChanged.AddListener(SetCrosshair);
        crosshairToggle.isOn = PlayerPrefs.GetInt("Crosshair") == 1;
        UnpauseGame();
    }

    private void SetCrosshair(bool arg0)
    {
        crosshair.SetActive(arg0);
        PlayerPrefs.SetInt("Crosshair", arg0 ? 1 : 0);
    }

    private void SetSensitivity(float input)
    {
        sensInputField.text = input.ToString();
        player.HorizontalSensitivity = input;
        player.VerticalSensitivity = input;
        PlayerPrefs.SetFloat("Sensitivity", input);
    }

    private void SetSensitivity(string input)
    {
        float value = float.Parse(input);
        sensSlider.value = value;
        player.HorizontalSensitivity = value;
        player.VerticalSensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
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
        pauseScreen.SetActive(true);
        CloseSettings();
        gamePaused = true;
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
        InputManager.ReloadInputs();
    }
}
