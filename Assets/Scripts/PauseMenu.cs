using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;
    [SerializeField] StatDisplayer statDisplayer;
    [SerializeField] Button backSettingsButton;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField volumeInputField;
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_InputField sensInputField;
    [SerializeField] Toggle crosshairToggle;
    [SerializeField] GameObject crosshair;
    [SerializeField] List<UniversalRenderPipelineAsset> graphics;


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
        sensInputField.text = value.ToString();

        value = PlayerPrefs.GetFloat("Volume") - 1;
        if (value + 1 == 0)
        {
            value = 100f;
            PlayerPrefs.SetFloat("Volume", value + 1);
        }
        AudioListener.volume = value / 100f;
        volumeSlider.value = value;
        volumeInputField.text = value.ToString();

        if(PlayerPrefs.GetInt("Graphics") == 0)
        {
            PlayerPrefs.SetInt("Graphics", 2);
        }

        graphicsDropdown.value = PlayerPrefs.GetInt("Graphics") - 1;
        QualitySettings.renderPipeline = graphics[PlayerPrefs.GetInt("Graphics") - 1];
        graphicsDropdown.onValueChanged.AddListener(SetGraphics);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeInputField.onValueChanged.AddListener(SetVolume);
        sensSlider.onValueChanged.AddListener(SetSensitivity);
        sensInputField.onValueChanged.AddListener(SetSensitivity);
        crosshair.SetActive(PlayerPrefs.GetInt("Crosshair") == 1);
        crosshairToggle.onValueChanged.AddListener(SetCrosshair);
        crosshairToggle.isOn = PlayerPrefs.GetInt("Crosshair") == 1;
        UnpauseGame();
    }

    private void SetGraphics(int value)
    {
        QualitySettings.renderPipeline = graphics[value];
        PlayerPrefs.SetInt("Graphics", value + 1);
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

    private void SetVolume(float input)
    {
        volumeInputField.text = input.ToString();
        AudioListener.volume = input / 100f;
        PlayerPrefs.SetFloat("Volume", input + 1);
    }

    private void SetVolume(string input)
    {
        float value = float.Parse(input);
        volumeSlider.value = value;
        AudioListener.volume = value / 100f;
        PlayerPrefs.SetFloat("Volume", value + 1);
    }

    private void OpenSettings()
    {
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        statDisplayer.gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    private void CloseSettings()
    {
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        statDisplayer.RefreshStats();
        statDisplayer.gameObject.SetActive(true);
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
