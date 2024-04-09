using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionTrigger : MonoBehaviour
{
    [SerializeField] Transform waitingRoomPosition;
    [SerializeField] Transform dungeonPosition;

    PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameSettings.Instance.PlayerInDungeon)
            {
                player.Teleport(dungeonPosition.position);
            }
            else
            {
                player.Teleport(waitingRoomPosition.position);
            }

            return;
        }

        other.transform.position = waitingRoomPosition.position;
    }
}
