using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    int health;

    public float invulnerabilityTime = 0.5f;
    private float lastDamage;

    Image bloodScreen;
    TextMeshProUGUI healthText;
    
    Coroutine fadeOutCoroutine;
    CameraShake cameraShake;
    public CameraShakePreset cameraShakePreset;

    private void Awake()
    {
        bloodScreen = GameObject.Find("BloodScreen")?.GetComponent<Image>();
        bloodScreen.color = new Color(1, 1, 1, 0);
        health = maxHealth;
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.OnWaveOver += () =>
            {
                health = maxHealth;
                UpdateHealthBar();
            };
        healthText = GameObject.Find("HealthNumber")?.GetComponent<TextMeshProUGUI>();
        cameraShake = CameraShake.Instance;
        UpdateHealthBar();
    }

    public void UpgradeHealth(int health)
    {
        maxHealth += health;
        this.health += health;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (lastDamage + invulnerabilityTime <= Time.time)
        {
            //Debug.Log("take damage");
            health -= damage;
            cameraShake.Shake(cameraShakePreset);
            if (health <= 0)
            {
                health = 0;
                UpdateHealthBar();
                Die();
            }
            else UpdateHealthBar();

            lastDamage = Time.time;
        }
    }

    public void Die()
    {
        SceneManager.LoadScene("DeathScene");
        Debug.Log("He do be dead");
    }

    void UpdateHealthBar()
    {
        float fill = health / (float)maxHealth;
        bloodScreen.color = new Color(1, 1, 1, 1 - fill);
        healthText.text = health + "/" + maxHealth;

        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);

        fadeOutCoroutine = StartCoroutine(fadeOut(fill));
    }

    IEnumerator fadeOut(float fill)
    {
        while (fill < 1)
        {
            yield return new WaitForSeconds(0.1f);
            fill += 0.001f;
            bloodScreen.color = new Color(1, 1, 1, 1 - fill);
        }
    }
}
