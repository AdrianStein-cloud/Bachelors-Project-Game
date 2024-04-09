using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportGun : CooldownItem
{
    public int range;
    public float chargeTime = 2f;

    bool isHeld = false;

    bool charging = false;
    bool canTP = false;

    Vector3 tpLocation;

    PlayerMovement movement;
    [SerializeField] AudioSource chargingSound;
    [SerializeField] AudioSource teleportSound;

    float startedCharging;

    private void Awake()
    {
        movement = transform.root.GetComponentInChildren<PlayerMovement>();
    }


    private void Update()
    {
        bool chargeTP = isHeld & IsOffCooldown && InputManager.Player.ItemPrimary.phase == InputActionPhase.Performed;
        if (chargeTP)
        {
            Charge();
        }

        bool teleport = 
              isHeld 
            & canTP 
            & charging 
            & Time.time >= startedCharging + chargeTime 
            & InputManager.Player.ItemPrimary.phase == InputActionPhase.Waiting;
     
        if (!chargeTP & teleport)
        {
            Teleport();
        }

        bool stoppedCharging = charging & InputManager.Player.ItemPrimary.phase != InputActionPhase.Performed;
        if (stoppedCharging)
        {
            StopCharging();
        }
    }

    void Teleport()
    {
        //Debug.Log("Triggerd");
        teleportSound.Play();
        movement.Teleport(tpLocation);
        StartCoroutine(Cooldown());
    }

    void Charge()
    {
        //Debug.Log("Held");
        tpLocation = transform.position + transform.forward * range;
        canTP = IsLocationTeleportable(tpLocation);
        UnitySingleton<TeleportTextController>.Instance.Display(canTP, Time.time >= startedCharging + chargeTime);
        if (!charging)
        {
            chargingSound.Play();
            startedCharging = Time.time;
            charging = true;

            InputManager.Player.Move.Disable();
        }
    }


    void StopCharging()
    {
        InputManager.Player.Move.Enable();
        UnitySingleton<TeleportTextController>.Instance.Hide();
        chargingSound.Stop();
        charging = false;
        canTP = false;
    }

    bool IsLocationTeleportable(Vector3 location)
    {
        if (Physics.Raycast(location, Vector3.down, out RaycastHit hit, Mathf.Infinity, ~LayerMask.GetMask("Water")))
        {
            var gameobject = hit.collider.gameObject;
            if (gameobject.layer == LayerMask.NameToLayer("Ground") 
                && gameobject.transform.root.name == "Dungeon"
                && GameSettings.Instance.PlayerInDungeon)
            {
                return true;
            }
        }
        return false;
    }
    public override void Select()
    {
        isHeld = true;
    }

    public override void Deselect()
    {
        isHeld = false;
    }
}
