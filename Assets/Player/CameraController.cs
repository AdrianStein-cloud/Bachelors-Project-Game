using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField, Range(50, 90)] int rotationAngle;
    [SerializeField] float followSpeed;
    [SerializeField] Transform cameraPosition;
    [SerializeField] Transform player;

    Vector2 dir;
    float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Look(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, followSpeed * Time.deltaTime);

        var mouseDir = mouseSensitivity * Time.deltaTime * dir;
        var yRotation = transform.localRotation.eulerAngles.y + mouseDir.x;

        xRotation -= mouseDir.y;
        xRotation = Mathf.Clamp(xRotation, -rotationAngle, rotationAngle);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.Rotate(Vector3.up * mouseDir.x);
    }
}
