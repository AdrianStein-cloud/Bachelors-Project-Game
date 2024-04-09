using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightAnimEvents : MonoBehaviour
{
    public FlashlightController controller;

    public void TryToggleFlashlight()
    {
        controller.TryToggleLight();
    }
}
