using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] float idleBobbingSpeed;
    [SerializeField] float idleBobbingAmount;
    [SerializeField] float walkingBobbingSpeed;
    [SerializeField] float walkingBobbingAmount;
    [SerializeField] float runningBobbingSpeed;
    [SerializeField] float runningBobbingAmount;
    [SerializeField] PlayerMovement movement;

    float currentBobbingSpeed;
    float currentBobbingAmount;
    float timer = 0;

    void Update()
    {
        currentBobbingSpeed = movement.IsRunning ? runningBobbingSpeed : (movement.IsWalking ? walkingBobbingSpeed : idleBobbingSpeed);
        currentBobbingAmount = movement.IsRunning ? runningBobbingAmount : (movement.IsWalking ? walkingBobbingAmount : idleBobbingAmount);

        timer += Time.deltaTime * currentBobbingSpeed;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Mathf.Sin(timer) * currentBobbingAmount, transform.localPosition.z);
    }
}
