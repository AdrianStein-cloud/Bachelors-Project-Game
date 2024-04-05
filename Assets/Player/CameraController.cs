using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

//https://github.com/deadlykam/TutorialFPSRotation/blob/20c94069f25b51205404a644a49f7b378506668e/TutorialFPSRotation/Assets/TutorialFPSRotation/Scripts/PlayerRotateSmooth.cs
public class CameraController : MonoBehaviour
{
    [field: SerializeField] public float HorizontalSensitivity { get; set; }
    [field: SerializeField] public float VerticalSensitivity { get; set; }
    public bool CameraFollowsPlayer { get; set; } = true;

    [SerializeField] float sensitivityMultiplier = 1f;

    [SerializeField, Range(50, 90)] int rotationAngle;
    [SerializeField] float followSmoothTime;
    [SerializeField] float runFOVChangeAmount;
    [SerializeField] float runFOVChangeSpeed;
    [SerializeField] Transform cameraPosition;
    [SerializeField] PlayerMovement player;

    [Header("Lean Properties")]
    [SerializeField] bool enableLeaning;
    [SerializeField] float leanAngle;
    [SerializeField] float leanTime;
    [SerializeField] float leanOffset;

    [Header("Smoothing Properties")]
    [SerializeField] float smoothTime;
    [SerializeField] Transform horiRotHelper;

    [Header("Stun Properties")]
    [SerializeField] float stunSensitivityMultiplier = 0.25f;

    Vector2 dir;
    Vector3 velocity = Vector3.zero;
    float xRotation = 0f;
    float currentFov;
    float startingSensitivityMultiplier;

    float xOld;
    float rotZ;
    float xAngularVelocity;
    float yAngularVelocity;

    bool leaningLeft;
    bool leaningRight;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentFov = Camera.main.fieldOfView;
        startingSensitivityMultiplier = sensitivityMultiplier;

        InputManager.Player.Look.OnAnyEvent(Look);

        horiRotHelper.localRotation = transform.localRotation;
        var camera = GetComponentInChildren<Camera>();
        UnitySingleton<CameraManager>.Instance.activeCameras.Add(camera);
        UnitySingleton<CameraManager>.Instance.cameraPositions.Add(camera, transform.root.GetComponentInChildren<CharacterController>().gameObject);
    }

    public void IncrementFov(float fov)
    {
        currentFov += fov;
    }

    private void Look(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    void LeanLeft()
    {
        if (InputManager.Player.LeanLeft.triggered)
        {
            StopAllCoroutines();
            leaningLeft = !leaningLeft;
            StartCoroutine(leaningLeft ? LerpLean(-leanOffset, leanAngle) : LerpLean(0f, 0f));
            leaningRight = false;
        }
    }

    void LeanRight()
    {
        if (InputManager.Player.LeanRight.triggered)
        {
            StopAllCoroutines();
            leaningRight = !leaningRight;
            StartCoroutine(leaningRight ? LerpLean(leanOffset, -leanAngle) : LerpLean(0f, 0f));
            leaningLeft = false;
        }
    }

    IEnumerator LerpLean(float positionValue, float rotationValue)
    {
        float elapsedTime = 0;
        var startPositionX = cameraPosition.localPosition.x;
        var startRotationZ = transform.eulerAngles.z;
        startRotationZ = startRotationZ > 180 ? -(360 - startRotationZ) : startRotationZ;

        while (elapsedTime < leanTime)
        {
            var pos = cameraPosition.localPosition;
            pos.x = Mathf.Lerp(startPositionX, positionValue, elapsedTime / leanTime);
            cameraPosition.localPosition = pos;

            rotZ = Mathf.Lerp(startRotationZ, rotationValue, elapsedTime / leanTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotZ = rotationValue;
        cameraPosition.localPosition = cameraPosition.localPosition.WithX(positionValue);
    }

    private void Update()
    {
        if (enableLeaning)
        {
            LeanLeft();
            LeanRight();
        }

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, currentFov * (player.IsRunning ? runFOVChangeAmount : 1f), runFOVChangeSpeed * Time.deltaTime);

        if (CameraFollowsPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, cameraPosition.position, ref velocity, followSmoothTime * Time.deltaTime);
        }

        var mouseDir = new Vector2(dir.x * HorizontalSensitivity, dir.y * VerticalSensitivity) * sensitivityMultiplier / 100;

        xOld = xRotation;
        xRotation -= mouseDir.y;
        xRotation = Mathf.SmoothDampAngle(xOld, xRotation, ref xAngularVelocity, smoothTime);
        xRotation = Mathf.Clamp(xRotation, -rotationAngle, rotationAngle);
        var yRotation = Mathf.SmoothDampAngle(transform.localEulerAngles.y, horiRotHelper.localEulerAngles.y, ref yAngularVelocity, smoothTime);
        //var yRotation = transform.localRotation.eulerAngles.y + mouseDir.x;

        horiRotHelper.Rotate(Vector3.up * mouseDir.x, Space.Self);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, rotZ);
        player.transform.Rotate(Vector3.up * mouseDir.x);
    }

    public void StartStun()
    {
        CameraShake.Instance.Shake(CameraShake.Instance.Defaultpreset);
        sensitivityMultiplier = stunSensitivityMultiplier;
    }

    public void EndStun()
    {
        sensitivityMultiplier = startingSensitivityMultiplier;
    }
}
