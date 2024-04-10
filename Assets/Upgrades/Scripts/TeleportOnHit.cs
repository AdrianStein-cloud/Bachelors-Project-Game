using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport On Hit Upgrade", menuName = "Upgrades/Teleport/OnHitAfterTeleport")]
public class TeleportOnHit : Upgrade
{
    [SerializeField] int secondsAfterTeleport;
    [SerializeField] AudioClip teleportSound;

    protected override object[] Args => new object[] { secondsAfterTeleport };

    Coroutine currentRoutine;
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;

    public override void Apply(GameObject playerObject)
    {
        playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
        playerMovement = playerObject.GetComponentInChildren<PlayerMovement>();
        Stats.Instance.teleport.OnTeleport += () =>
        {
            if (currentRoutine != null)
            {
                playerHealth.StopCoroutine(currentRoutine);
                playerHealth.OnTakeDamage -= TeleportToRandomLocation;
            }
            currentRoutine = playerHealth.StartCoroutine(EnableTeleportOnTakeDamge());
        };
    }

    IEnumerator EnableTeleportOnTakeDamge()
    {
        playerHealth.OnTakeDamage += TeleportToRandomLocation;
        yield return new WaitForSeconds(secondsAfterTeleport);
        playerHealth.OnTakeDamage -= TeleportToRandomLocation;
        currentRoutine = null;
    }

    void TeleportToRandomLocation(int _)
    {
        var rooms = UnitySingleton<Dungeon>.Instance.Rooms;
        var randomRoom = rooms[Random.Range(0, rooms.Count)];
        var position = randomRoom.centerObject.transform.position + Vector3.up * 6;
        playerMovement.Teleport(position);
        AudioSource.PlayClipAtPoint(teleportSound, position);
    }
}
