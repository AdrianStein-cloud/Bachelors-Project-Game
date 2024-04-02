using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportGun : Item
{
    public int range;
    public float chargeTime = 2f;
    public int cooldown = 30;

    bool isHeld = false;

    bool charging = false;
    bool canTP = false;
    bool isOffCooldown = true;

    float usedTime;

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
        bool chargeTP = isHeld & isOffCooldown && InputManager.Player.ItemPrimary.phase == InputActionPhase.Performed;
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

        if (!isOffCooldown)
        {
            UnitySingleton<Inventory>.Instance.UpdateItemText(this, (cooldown - (Time.time - usedTime)).ToString("N0"));
        }
    }

    void Teleport()
    {
        //Debug.Log("Triggerd");
        teleportSound.Play();
        movement.Teleport(tpLocation);
        usedTime = Time.time;
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
        if (Physics.Raycast(location, Vector3.down, out RaycastHit hit, Mathf.Infinity))
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

    IEnumerator Cooldown()
    {
        isOffCooldown = false;
        yield return new WaitForSeconds(cooldown);
        isOffCooldown = true;
        UnitySingleton<Inventory>.Instance.UpdateItemText(this, "");
    }
}
