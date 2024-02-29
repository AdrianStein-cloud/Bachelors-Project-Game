using BBUnity.Conditions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Condition("CanAttack")]
public class CanAttack : GOCondition
{
    public override bool Check()
    {
        var wander = gameObject.GetComponent<WanderingBehaviour>();
        return ((wander.lastAttackTime + wander.attackDelay) <= Time.time);
    }
}
