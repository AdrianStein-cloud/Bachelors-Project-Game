using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Action("DamagePlayer")]
public class DamagePlayer : GOAction
{
    [InParam("damage")]
    public int damage;

    [InParam("target")]
    public GameObject target;

    public override void OnStart()
    {
        target.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    public override TaskStatus OnUpdate()
    {
        return base.OnUpdate();
    }
}
