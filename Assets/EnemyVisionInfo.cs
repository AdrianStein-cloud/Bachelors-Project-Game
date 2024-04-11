using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionInfo : MonoBehaviour
{
    public EnemyType type;
    public bool CanSeePlayer { get; set; }

    public bool effectRecently;


    public IEnumerator StartVisionCooldown(float cd)
    {
        effectRecently = true;
        yield return new WaitForSeconds(cd);
        effectRecently = false;
    }
}

public enum EnemyType
{
    wanderer,
    coil,
    wolf
}
