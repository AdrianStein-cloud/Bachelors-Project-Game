using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputActions Actions;
    public static InputActions.PlayerActions Player => Actions.Player;

    private void Awake()
    {
        Actions = new InputActions();
        Actions.Enable();
    }
}
