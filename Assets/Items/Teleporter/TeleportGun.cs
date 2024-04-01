using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportGun : Item
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

    GameObject location;

    float startedCharging;

    private void Awake()
    {
        movement = transform.root.GetComponentInChildren<PlayerMovement>();
        location = new GameObject("Location");
    }


    private void Update()
    {
        if (isHeld && InputManager.Player.ItemPrimary.phase == InputActionPhase.Performed)
        {
            Debug.Log("Held");
            tpLocation = transform.position + transform.forward * range;
            location.transform.position = tpLocation;
            canTP = IsLocationTeleportable(tpLocation);
            if (!charging)
            {
                chargingSound.Play();
                startedCharging = Time.time;
            }
            charging = true;
        }
        else if (isHeld & canTP & charging & Time.time >= startedCharging + chargeTime & InputManager.Player.ItemPrimary.phase == InputActionPhase.Waiting)
        {
            Debug.Log("Triggerd");
            teleportSound.Play();
            movement.Teleport(tpLocation);
        }

        if (charging & InputManager.Player.ItemPrimary.phase != InputActionPhase.Performed)
        {
            chargingSound.Stop();
            charging = false;
            canTP = false;
        }

    }

    public override void Select()
    {
        isHeld = true;
    }

    public override void Deselect()
    {
        isHeld = false;
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
}
