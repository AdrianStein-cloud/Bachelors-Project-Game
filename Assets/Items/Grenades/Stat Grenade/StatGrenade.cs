using UnityEngine;

public class StatGrenade : EffectGrenade<(PlayerHealth health, PlayerMovement movement)>
{
    protected override LayerMask Mask => LayerMask.GetMask("Player");

    protected override void StartEffect((PlayerHealth, PlayerMovement) target)
    {
        throw new System.NotImplementedException();
    }

    protected override void EndEffect((PlayerHealth, PlayerMovement) target)
    {
        throw new System.NotImplementedException();
    }

    protected override bool TryExtractTargetComponent(Collider collider, out (PlayerHealth health, PlayerMovement movement) target)
    {
        var health = collider.GetComponent<PlayerHealth>();
        var movement = collider.GetComponent<PlayerMovement>();

        if(health != null & movement != null)
        {
            target = (health, movement);
            return true;
        }
        else
        {
            target = (null, null);
            return false;
        }
    }
}
