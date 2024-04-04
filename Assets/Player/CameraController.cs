using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    float xAngularVelocity;
    float yAngularVelocity;

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

    public void LerpPosition(Vector3 position, float smoothTime)
    {
        CameraFollowsPlayer = false;
        StartCoroutine(LerpPositionCoroutine(position, smoothTime));
    }

    IEnumerator LerpPositionCoroutine(Vector3 position, float smoothTime)
    {
        float elapsedTime = 0;
        var startValue = transform.position;
        while (elapsedTime < smoothTime)
        {
            transform.position = Vector2.Lerp(startValue, position, elapsedTime / smoothTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private void Update()
    {
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
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
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
