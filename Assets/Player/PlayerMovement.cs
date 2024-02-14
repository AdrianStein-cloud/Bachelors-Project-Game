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

    [Header("Movement Settings")]
    [SerializeField] bool toggleRun;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float crouchSpeed;

    [Header("Crouch Settings")]
    [SerializeField] bool toggleCrouch;
    [SerializeField] float crouchHeight;
    [SerializeField] float cameraCrouchOffset;
    [SerializeField] float smoothCrouchTime;
    [SerializeField] float smoothCrouchSpeed;
    [SerializeField] float ceilingCheckDistance;

    [Header("Jump and Gravity")]
    [SerializeField] bool enableJump = true;
    [SerializeField] float jumpHeight;
    [SerializeField] float groundCheckRadius = 0.4f;
    [SerializeField] float gravity;
    [SerializeField] float airGravity;

    CharacterController controller;
    new Audio audio;
    Vector3 velocity;
    Vector2 dir;
    float currentSpeed;
    float airSpeed;
    Vector3 cameraPosition;
    Vector3 groundCheckPosition;
    float controllerHeight;
    bool isGrounded;
    bool canStand;
    bool ungrounded;
    bool jumping;
    bool run;

    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        audio = GetComponent<Audio>();
        airSpeed = currentSpeed = walkSpeed;
        cameraPosition = cam.localPosition;
        controllerHeight = controller.height;
        groundCheckPosition = groundCheck.localPosition;
    }

    private void Start()
    {
        var input = InputManager.Player;
        input.Move.OnAnyEvent(Move);
        input.Jump.OnAnyEvent(Jump);
        input.Run.OnAnyEvent(Run);
        input.Crouch.OnAnyEvent(Crouch);
    }

    private void Update()
    {
        if (run && dir.y > 0 && isGrounded)
        {
            StopCrouch();
            IsRunning = true;
        }
        else IsRunning = false;

        if (dir.y <= 0 && toggleRun) run = false;

        currentSpeed = IsRunning ? runSpeed : (IsCrouching ? crouchSpeed : walkSpeed);
        IsWalking = dir.magnitude > 0f && !IsRunning && isGrounded;

        canStand = !Physics.Raycast(transform.position, Vector3.up, ceilingCheckDistance) && IsCrouching;

        var move = transform.right * dir.x + transform.forward * dir.y;
        controller.Move(Time.deltaTime * ((isGrounded ? currentSpeed : airSpeed) * move + velocity));

        Gravity();

        if (isGrounded && IsWalking && !audio.IsPlaying("Walk")) 
        {
            audio.Pause("Run");
            audio.Play("Walk");
        }
        else if (isGrounded && IsRunning && !audio.IsPlaying("Run"))
        {
            audio.Pause("Walk");
            audio.Play("Run");
        }
        else if (!IsWalking && !IsRunning || !isGrounded)
        {
            audio.Pause("Run");
            audio.Pause("Walk");
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void Run(InputAction.CallbackContext context)
    {
        if (toggleRun && !context.performed || context.started) return;
        run = context.performed;

        //StopCrouch();
        //IsRunning = isGrounded && !IsCrouching && dir.y > 0 && (toggleRun || context.performed);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!enableJump || jumping || !isGrounded || !context.performed) return;
        StopCrouch();
        jumping = true;
        velocity.y = Mathf.Sqrt(jumpHeight * 2f * airGravity);
        airSpeed = currentSpeed;
        Invoke(nameof(ResetJump), 0.2f);
    }

    private void ResetJump() => jumping = false;

    private void Crouch(InputAction.CallbackContext context)
    {
        if (toggleCrouch && !context.performed || context.started || (IsCrouching && !canStand)) return;
        IsCrouching = isGrounded && (toggleCrouch ? !IsCrouching : context.performed);
        IsRunning = false;
        run = false;
        StartCoroutine(SmoothCrouch());
    }

    private IEnumerator SmoothCrouch()
    {
        var time = 0f;

        var cameraStart = cam.localPosition;
        var cameraEnd = IsCrouching ? cameraPosition + new Vector3(0f, cameraCrouchOffset, 0f) : cameraPosition;

        var groundStart = groundCheck.localPosition;
        var groundEnd = IsCrouching ? groundCheckPosition + new Vector3(0f, (controllerHeight - crouchHeight) / 2f, 0f) : groundCheckPosition;

        var heightStart = controller.height;
        var heightEnd = IsCrouching ? crouchHeight : controllerHeight;

        while (time < smoothCrouchTime)
        {
            time += Time.deltaTime * smoothCrouchSpeed;
            cam.localPosition = Vector3.Lerp(cameraStart, cameraEnd, time);
            groundCheck.localPosition = Vector3.Lerp(groundStart, groundEnd, time);
            controller.height = Mathf.Lerp(heightStart, heightEnd, time);
            yield return null;
        }
    }

    private void StopCrouch()
    {
        if (IsCrouching && !canStand) return;
        IsCrouching = false;
        StartCoroutine(SmoothCrouch());
    }

    private void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if ((isGrounded || !isGrounded && !ungrounded) && velocity.y < 0)
        {
            velocity.y = -2f;
            ungrounded = !isGrounded;
        }

        velocity.y -= (isGrounded && !jumping ? gravity : airGravity) * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
