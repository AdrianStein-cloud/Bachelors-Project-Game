using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownItem : Item
{
    public int cooldown;

    protected bool IsOffCooldown { get; private set; } = true;

    float usedTime;

    float usedCooldown;

    private void FixedUpdate()
    {
        if (!IsOffCooldown)
        {
            UnitySingleton<Inventory>.Instance.UpdateItemText(this, (usedCooldown - (Time.time - usedTime)).ToString("N0"));
        }
    }

    protected IEnumerator Cooldown()
    {
        usedTime = Time.time;
        IsOffCooldown = false;
        usedCooldown = cooldown * Stats.Instance.cooldown.CalculatedRecoverySpeedMultiplier;
        yield return new WaitForSeconds(usedCooldown);
        IsOffCooldown = true;
        UnitySingleton<Inventory>.Instance.UpdateItemText(this, "");
    }
}
