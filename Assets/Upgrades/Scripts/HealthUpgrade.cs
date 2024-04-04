using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Upgrade", menuName = "Upgrades/Health Upgrade")]
public class HeealthUpgrade : Upgrade
{
    [SerializeField] int health;

    protected override object[] Args => new object[] { health };

    public override void Apply(GameObject playerObject)
    {
        var player = playerObject.GetComponentInChildren<PlayerHealth>();
        player.AddMaxHealth(health);
    }
}
