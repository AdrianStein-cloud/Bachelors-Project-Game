using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] bool enableStamina = true;
    [SerializeField] public float stamina;
    [SerializeField] public float staminaDrainSpeed;
    [SerializeField] public float staminaRecoverySpeed;
    [SerializeField] public float staminaRecoveryDelay;

    public bool SufficientStamina => currentStamina > 0 || !enableStamina;

    Transform fill;
    PlayerMovement player;

    float currentStamina;
    float lastRecoveryTime;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        fill = GameObject.Find("Canvas").transform.Find("StaminaBar").GetChild(0);
        fill.gameObject.SetActive(false);
        currentStamina = stamina;
    }

    private void Update()
    {
        if (player.IsRunning)
        {
            currentStamina -= staminaDrainSpeed * Time.deltaTime;
            lastRecoveryTime = Time.time;
        }
        else if (Time.time > lastRecoveryTime + staminaRecoveryDelay)
        {
            currentStamina += staminaRecoverySpeed * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);

        fill.gameObject.SetActive(currentStamina != stamina);

        var scale = fill.localScale;
        scale.x = currentStamina / stamina;
        fill.localScale = scale;
    }
}
