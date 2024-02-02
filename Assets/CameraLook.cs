using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField, Range(50, 90)] int rotationAngle;
    [SerializeField] Transform playerBody;

    Vector2 dir;
    float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputValue value)
    {
        dir = value.Get<Vector2>();
    }

    private void Update()
    {
        var mouseDir = mouseSensitivity * Time.deltaTime * dir;

        xRotation -= mouseDir.y;
        xRotation = Mathf.Clamp(xRotation, -rotationAngle, rotationAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseDir.x);
    }
}
