using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownItem : Item
{
    public int cooldown;

    protected bool IsOffCooldown { get; private set; } = true;

    float usedTime;

    private void FixedUpdate()
    {
        if (!IsOffCooldown)
        {
            UnitySingleton<Inventory>.Instance.UpdateItemText(this, (cooldown - (Time.time - usedTime)).ToString("N0"));
        }
    }

    protected IEnumerator Cooldown()
    {
        usedTime = Time.time;
        IsOffCooldown = false;
        yield return new WaitForSeconds(cooldown);
        IsOffCooldown = true;
        UnitySingleton<Inventory>.Instance.UpdateItemText(this, "");
    }
}
