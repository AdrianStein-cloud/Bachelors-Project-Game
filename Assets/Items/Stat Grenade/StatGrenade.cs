using UnityEngine;

public class StatGrenade : EffectGrenade<(PlayerHealth health, PlayerMovement movement)>
{
    protected override void StartEffect((PlayerHealth, PlayerMovement) target)
    {
        throw new System.NotImplementedException();
    }

    protected override void EndEffect((PlayerHealth, PlayerMovement) target)
    {
        throw new System.NotImplementedException();
    }

    protected override (PlayerHealth health, PlayerMovement movement) ExtractTargetComponent(Collider collider)
    {
        var health = collider.GetComponent<PlayerHealth>();
        var movement = collider.GetComponent<PlayerMovement>();

        return (health, movement);
    }

    protected override bool IsColliderTarget(Collider collider) => collider.CompareTag("Player");

}
