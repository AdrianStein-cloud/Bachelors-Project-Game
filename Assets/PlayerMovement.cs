using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    CharacterController controller;
    Vector3 velocity;
    Vector2 dir;
    bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
    }

    private void Update()
    {
        var move = transform.right * dir.x + transform.forward * dir.y;
        controller.Move(move * speed * Time.deltaTime);

        Gravity();
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
