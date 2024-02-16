using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//https://github.com/deadlykam/TutorialFPSRotation/blob/20c94069f25b51205404a644a49f7b378506668e/TutorialFPSRotation/Assets/TutorialFPSRotation/Scripts/PlayerRotateSmooth.cs
public class CameraController : MonoBehaviour
{
    [field: SerializeField] public float HorizontalSensitivity { get; set; }
    [field: SerializeField] public float VerticalSensitivity { get; set; }

    [SerializeField, Range(50, 90)] int rotationAngle;
    [SerializeField] float followSmoothTime;
    [SerializeField] float runFOVChangeAmount;
    [SerializeField] float runFOVChangeSpeed;
    [SerializeField] Transform cameraPosition;
    [SerializeField] PlayerMovement player;

    [Header("Smoothing Properties")]
    [SerializeField] float smoothTime;
    [SerializeField] Transform horiRotHelper;

    Vector2 dir;
    Vector3 velocity = Vector3.zero;
    float xRotation = 0f;
    float startingFOV;

    float xOld;
    float xAngularVelocity;
    float yAngularVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingFOV = Camera.main.fieldOfView;
        InputManager.Player.Look.OnAnyEvent(Look);

        horiRotHelper.localRotation = transform.localRotation;
    }

    private void Look(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startingFOV * (player.IsRunning ? runFOVChangeAmount : 1f), runFOVChangeSpeed * Time.deltaTime);

        transform.position = Vector3.SmoothDamp(transform.position, cameraPosition.position, ref velocity, followSmoothTime * Time.deltaTime);

        var mouseDir = new Vector2(dir.x * HorizontalSensitivity, dir.y * VerticalSensitivity) * Time.deltaTime;

        xOld = xRotation;
        xRotation -= mouseDir.y;
        xRotation = Mathf.SmoothDampAngle(xOld, xRotation, ref xAngularVelocity, smoothTime);
        xRotation = Mathf.Clamp(xRotation, -rotationAngle, rotationAngle);
        var yRotation = Mathf.SmoothDampAngle(transform.localEulerAngles.y, horiRotHelper.localEulerAngles.y, ref yAngularVelocity, smoothTime);
        //var yRotation = transform.localRotation.eulerAngles.y + mouseDir.x;

        horiRotHelper.Rotate(Vector3.up * mouseDir.x, Space.Self);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.transform.Rotate(Vector3.up * mouseDir.x);
    }
}
