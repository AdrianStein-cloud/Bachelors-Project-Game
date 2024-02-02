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
    [SerializeField] PlayerMovement player;

    float currentBobbingSpeed;
    float currentBobbingAmount;
    float timer = 0;

    void Update()
    {
        currentBobbingSpeed = player.IsRunning ? runningBobbingSpeed : (player.IsWalking ? walkingBobbingSpeed : idleBobbingSpeed);
        currentBobbingAmount = player.IsRunning ? runningBobbingAmount : (player.IsWalking ? walkingBobbingAmount : idleBobbingAmount);

        timer += Time.deltaTime * currentBobbingSpeed;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Mathf.Sin(timer) * currentBobbingAmount, transform.localPosition.z);
    }
}
