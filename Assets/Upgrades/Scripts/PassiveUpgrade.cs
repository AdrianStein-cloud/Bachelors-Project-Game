using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive Upgrade", menuName = "Upgrades/Passive Upgrade")]
public class PassiveUpgrade : Upgrade
{
    public GameObject passive;

    public override void Apply(GameObject player)
    {
        Instantiate(passive, player.transform);
    }
}
