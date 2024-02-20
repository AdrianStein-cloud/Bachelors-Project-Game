using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth { get; private set; }

    public float invulnerabilityTime = 0.5f;
    private float lastDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void UpgradeHealth(int health)
    {
        maxHealth += health;
        currentHealth += health;
    }

    public void TakeDamage(int damage)
    {
        if (lastDamage + invulnerabilityTime < Time.time)
        {
            Debug.Log("take damage");
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        lastDamage = Time.time;
    }

    public void Die()
    {
        Debug.Log("He do be dead");
    }
}
