using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] bool enableStamina = true;
    [SerializeField] float stamina;
    [SerializeField] float drainSpeed;
    [SerializeField] float jumpStaminaUse;
    [SerializeField] float recoverySpeed;
    [SerializeField] float recoveryDelay;
    [SerializeField] float recoveryDelayAtZero;

    public bool SufficientStamina => currentStamina > 0 || !enableStamina;

    Image fill;
    PlayerMovement player;

    float currentStamina;
    float lastRecoveryTime;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        fill = GameObject.Find("Canvas").transform.Find("StaminaBar").GetChild(0).GetComponent<Image>();
        fill.gameObject.SetActive(false);
        currentStamina = stamina;

        player.OnJump += () =>
        {
            currentStamina -= jumpStaminaUse;
            lastRecoveryTime = Time.time;
        };
    }

    private void Update()
    {
        if (player.IsRunning)
        {
            currentStamina -= drainSpeed * Time.deltaTime;
            lastRecoveryTime = Time.time;
        }
        else if (Time.time > lastRecoveryTime + (SufficientStamina ? recoveryDelay : recoveryDelayAtZero))
        {
            currentStamina += recoverySpeed * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);

        fill.gameObject.SetActive(currentStamina != stamina);

        fill.fillAmount = currentStamina / stamina;
    }

    public void UpgradeStamina(float staminaMultiplier, float recoverySpeedMultiplier)
    {
        stamina *= staminaMultiplier / 100 + 1;
        recoverySpeed *= recoverySpeedMultiplier / 100 + 1;
        currentStamina = stamina;
    }
}
