using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    int health;

    public float invulnerabilityTime = 0.5f;
    private float lastDamage;

    Image healthBarFill;
    TextMeshProUGUI healthText;

    private void Awake()
    {
        health = maxHealth;
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.OnWaveOver += () =>
            {
                health = maxHealth;
                UpdateHealthBar();
            };
        var healthBar = GameObject.Find("HealthBar").transform;
        healthBarFill = healthBar.Find("Fill").GetComponent<Image>();
        healthText = healthBar.Find("Number").GetComponent<TextMeshProUGUI>();
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
        if (lastDamage + invulnerabilityTime < Time.time)
        {
            Debug.Log("take damage");
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                UpdateHealthBar();
                Die();
            }
            else UpdateHealthBar();
        }

        lastDamage = Time.time;
    }

    public void Die()
    {
        SceneManager.LoadScene("DeathScene");
        Debug.Log("He do be dead");
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = health / (float)maxHealth;
        healthText.text = health.ToString();
    }
}
