using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Condition("TrueCondition")]
public class TrueCondition : GOCondition
{
    public override bool Check()
    {
        return true;
    }
}
