using System;
using System.Collections;
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

    public Action<int> OnTakeDamage;

    Image bloodScreen;
    TextMeshProUGUI healthText;
    
    Coroutine fadeOutCoroutine;
    public CameraShakePreset cameraShakePreset;

    [SerializeField] private AudioSource hitSource, dieSource;

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
        UpdateHealthBar();
    }

    public void AddMaxHealth(int health)
    {
        maxHealth += health;
        this.health += health;
        UpdateHealthBar();
    }

    public void RemoveMaxHealth(int health)
    {
        maxHealth -= health;
        if (this.health > maxHealth) this.health = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (lastDamage + invulnerabilityTime <= Time.time)
        {
            //Debug.Log("take damage");
            health -= damage;
            CameraShake.Instance.Shake(cameraShakePreset);
            hitSource.Play();
            if (health <= 0)
            {
                health = 0;
                UpdateHealthBar();
                StartCoroutine(WaitBeforeDeath());
            }
            else UpdateHealthBar();

            if(damage > 0) OnTakeDamage?.Invoke(damage);

            lastDamage = Time.time;
        }
    }

    IEnumerator WaitBeforeDeath()
    {
        dieSource.Play();
        yield return new WaitForSeconds(0.2f);
        Die();
    }

    public void Die()
    {
        PlayerPrefs.SetInt("player_score_" + GameSettings.Instance.DifficultyConfig.difficulty, GameSettings.Instance.Wave);

        SceneManager.LoadScene("DeathScene");
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
