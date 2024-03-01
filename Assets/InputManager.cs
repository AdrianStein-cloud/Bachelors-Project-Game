using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputActions Actions { get; private set; }
    public static InputActions.PlayerActions Player => Actions.Player;

    private void Awake()
    {
        ReloadInputs();
        SceneManager.sceneLoaded += (_, _) => ReloadInputs();
    }

    public static void ReloadInputs()
    {
        Actions?.Dispose();
        Actions = new InputActions();
        Actions.Enable();
    }
}
