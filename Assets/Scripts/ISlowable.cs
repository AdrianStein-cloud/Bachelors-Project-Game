using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowable
{
    public void SlowDown(float slowFactor);
    public void ResetSpeed();
}
