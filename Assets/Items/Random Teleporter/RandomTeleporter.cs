using UnityEngine;

public class RandomTeleporter : QuantityItem
{
    PlayerMovement movement;
    AudioSource teleportSound;

    private void Awake()
    {
        movement = transform.root.GetComponentInChildren<PlayerMovement>();
        teleportSound = GetComponent<AudioSource>();
    }

    public override void Primary()
    {
        bool inDungeon = GameSettings.Instance.PlayerInDungeon;
        if (QuantityLeft & inDungeon)
        {
            var rooms = UnitySingleton<Dungeon>.Instance.Rooms;
            var randomRoom = rooms[Random.Range(0, rooms.Count)];
            var position = randomRoom.centerObject.transform.position + Vector3.up * 6;
            movement.Teleport(position);
            Stats.Instance.teleport.OnTeleport?.Invoke();
            teleportSound.Play();
            SpendQuantity();
        }
    }
}
