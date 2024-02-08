using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] Transform cam;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Movement Speeds")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float crouchSpeed;

    [Header("Crouch Settings")]
    [SerializeField] float crouchHeight;
    [SerializeField] float cameraCrouchOffset;
    [SerializeField] float smoothCrouchTime;
    [SerializeField] float smoothCrouchSpeed;

    [Header("Other")]
    [SerializeField] bool toggleRun;
    [SerializeField] bool toggleCrouch;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;

    CharacterController controller;
    Vector3 velocity;
    Vector2 dir;
    float currentSpeed;
    float airSpeed;
    Vector3 cameraPosition;
    float controllerHeight;
    bool isGrounded;

    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
        cameraPosition = cam.localPosition;
        controllerHeight = controller.height;
    }

    public void Move(InputAction.CallbackContext context) 
    {
        dir = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (dir.y <= 0) IsRunning = false;
        currentSpeed = IsRunning ? runSpeed : (IsCrouching ? crouchSpeed : walkSpeed);
        IsWalking = dir.magnitude > 0f && !IsRunning && isGrounded;

        var move = transform.right * dir.x + transform.forward * dir.y;
        controller.Move(Time.deltaTime * ((isGrounded ? currentSpeed : airSpeed) * move + velocity));

        Gravity();
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (toggleRun && !context.performed || context.started) return;
        StopCrouch();
        IsRunning = isGrounded && !IsCrouching && dir.y > 0 && (toggleRun || context.performed);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded || !context.performed) return;
        StopCrouch();
        velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        airSpeed = currentSpeed;
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (toggleCrouch && !context.performed || context.started) return;
        IsCrouching = isGrounded && (toggleCrouch ? !IsCrouching : context.performed);
        IsRunning = false;
        StartCoroutine(SmoothCrouch());
    }

    IEnumerator SmoothCrouch()
    {
        var time = 0f;
        Vector3 cameraStart = cam.localPosition;
        Vector3 cameraEnd = IsCrouching ? cameraPosition + new Vector3(0f, cameraCrouchOffset, 0f) : cameraPosition;
        var heightStart = controller.height;
        var heightEnd = IsCrouching ? crouchHeight : controllerHeight;
        while (time < smoothCrouchTime)
        {
            time += Time.deltaTime * smoothCrouchSpeed;
            cam.localPosition = Vector3.Lerp(cameraStart, cameraEnd, time);
            controller.height = Mathf.Lerp(heightStart, heightEnd, time);
            yield return null;
        }
    }

    void StopCrouch()
    {
        IsCrouching = false;
        StartCoroutine(SmoothCrouch());
    }

    void Gravity()
    {
        isGrounded = controller.isGrounded; // Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y -= gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);
    }
}
