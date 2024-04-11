using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionInfo : MonoBehaviour
{
    public EnemyType type;
    public bool CanSeePlayer { get; set; }

    public bool effectRecently;

    PlayerVisualEffects effects;

    private void Start()
    {
        effects = FindObjectOfType<PlayerVisualEffects>();
    }

    public IEnumerator StartVisionCooldown(float cd)
    {
        effectRecently = true;
        yield return new WaitForSeconds(cd);
        effectRecently = false;
    }

    public void BeginEffect()
    {
        if (!effectRecently)
        {
            effects.BeginChaseEffect(this);
            StartCoroutine(StartVisionCooldown(effects.effectCooldown));
        }
    }

    public void EndEffect()
    {
        effects.EndChaseEffect(this);
    }
}

public enum EnemyType
{
    wanderer,
    coil,
    wolf
}
