using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField, Range(50, 90)] int rotationAngle;
    [SerializeField] float followSpeed;
    [SerializeField] float runFOVChangeAmount;
    [SerializeField] float runFOVChangeSpeed;
    [SerializeField] Transform cameraPosition;
    [SerializeField] PlayerMovement player;

    Vector2 dir;
    float xRotation = 0f;
    float startingFOV;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingFOV = Camera.main.fieldOfView;
    }

    public void Look(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startingFOV * (player.IsRunning ? runFOVChangeAmount : 1f), runFOVChangeSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, followSpeed * Time.deltaTime);

        var mouseDir = mouseSensitivity * Time.deltaTime * dir;
        var yRotation = transform.localRotation.eulerAngles.y + mouseDir.x;

        xRotation -= mouseDir.y;
        xRotation = Mathf.Clamp(xRotation, -rotationAngle, rotationAngle);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.transform.Rotate(Vector3.up * mouseDir.x);
    }
}
