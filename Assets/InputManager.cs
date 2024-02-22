using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputActions Actions;
    public static InputActions.PlayerActions Player => Actions.Player;

    private void Awake() => ReloadInputs();


    public static void ReloadInputs()
    {
        Actions?.Dispose();
        Actions = new InputActions();
        Actions.Enable();
    }
}
