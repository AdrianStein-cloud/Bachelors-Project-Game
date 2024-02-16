using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Procedural Generation");
    }
}
