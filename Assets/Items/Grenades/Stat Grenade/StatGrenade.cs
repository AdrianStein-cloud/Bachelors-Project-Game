using UnityEngine;

public class StatGrenade : EffectGrenade<(PlayerHealth health, PlayerMovement movement, PlayerStamina stamina, CameraController cameraController)>
{
    public float movespeedMultiplier;
    public float maxHealthMultiplier;

    public int fovIncrease = 10;

    int extraMaxHealth;

    protected override LayerMask Mask => LayerMask.GetMask("Player");

    protected override void StartEffect((PlayerHealth health, PlayerMovement movement, PlayerStamina stamina, CameraController cameraController) target)
    {
        target.movement.SlowDown(movespeedMultiplier);
        extraMaxHealth = (int)(target.health.maxHealth * (maxHealthMultiplier - 1));
        target.health.AddMaxHealth(extraMaxHealth);
        target.stamina.SetStaminaEnabled(false);
        target.cameraController.IncrementFov(fovIncrease);

    }

    protected override void EndEffect((PlayerHealth health, PlayerMovement movement, PlayerStamina stamina, CameraController cameraController) target)
    {
        target.movement.ResetSpeed(movespeedMultiplier);
        target.health.RemoveMaxHealth(extraMaxHealth);
        target.stamina.SetStaminaEnabled(true);
        target.cameraController.IncrementFov(-fovIncrease);
    }

    protected override bool TryExtractTargetComponent(Collider collider, out (PlayerHealth health, PlayerMovement movement, PlayerStamina stamina, CameraController cameraController) target)
    {
        var health = collider.GetComponent<PlayerHealth>();
        var movement = collider.GetComponent<PlayerMovement>();
        var stamina = collider.GetComponent<PlayerStamina>();
        var cameraController = collider.transform.root.GetComponentInChildren<CameraController>();

        if (health != null & movement != null & stamina != null)
        {
            target = (health, movement, stamina, cameraController);
            return true;
        }
        else
        {
            target = (null, null, null, null);
            return false;
        }
    }
}
