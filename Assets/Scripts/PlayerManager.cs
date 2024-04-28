using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Action<GameObject> onPlayerSpawned;

    private void Awake()
    {
        UnitySingleton<PlayerManager>.BecomeSingleton(this);
    }

    public void OnPlayerSpawned(Action<GameObject> onSpawned)
    {
        onPlayerSpawned += onSpawned;
    }

    public void SpawnedPlayer(GameObject player)
    {
        onPlayerSpawned?.Invoke(player);
    }
}
