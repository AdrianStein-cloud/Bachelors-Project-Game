using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DungeonGenerator dungeonGenerator;
    GameObject player;
    ObjectiveSpawner objectiveSpawner;
    SimpleEnemySpawner enemySpawner;
    UpgradeManager upgradeManager;
    GameObject waitingRoomSpawnPoint;
    DungeonEntrance dungeonEntrance;
    DangerScaler dangerScaler;

    public Action<int> OnDungeonGenerated;
    public Action OnWaveOver;

    GameObject dungeon;

    private void Start()
    {
        waitingRoomSpawnPoint = GameObject.Find("WaitingRoomSpawnPoint");
        player = GameObject.FindGameObjectWithTag("Player");
        dungeonGenerator = GetComponent<DungeonGenerator>();
        objectiveSpawner = GetComponent<ObjectiveSpawner>();
        enemySpawner = GetComponent<SimpleEnemySpawner>();
        upgradeManager = GetComponent<UpgradeManager>();
        dungeonEntrance = FindAnyObjectByType<DungeonEntrance>();
        dungeonEntrance.EnterDungeon = StartLevel;
        player.transform.position = waitingRoomSpawnPoint.transform.position;
        dangerScaler = new DangerScaler();
        StartCoroutine(SpawnDungeon());
    }

    IEnumerator SpawnDungeon()
    {
        dungeonEntrance.DungeonIsAvailable = false;
        dungeon = new GameObject("Dungeon");
        dangerScaler.ScaleDanger();
        yield return dungeonGenerator.GenerateDungeon(dungeon.transform, GameSettings.Instance.CurrentDepth);
        dungeonEntrance.DungeonIsAvailable = true;
        OnDungeonGenerated?.Invoke(GameSettings.Instance.Wave);
    }

    void StartLevel()
    {
        enemySpawner.SpawnEnemies(dungeonGenerator.spawnedRooms, dungeon.transform, GameSettings.Instance.CurrentDepth - 1);
        objectiveSpawner.SpawnObjectives(dungeonGenerator.spawnedRoomsDepth, dungeon.transform, SwitchToUpgrades);
        player.SetActive(false);
        player.transform.position = dungeonGenerator.playerSpawnPosition.transform.position;
        player.SetActive(true);
    }

    public void SwitchToUpgrades(int upgrades)
    {
        InputManager.Player.Disable();
        player.SetActive(false);
        player.transform.position = waitingRoomSpawnPoint.transform.position;
        player.SetActive(true);
        Destroy(dungeon);
        OnWaveOver?.Invoke();
        StartCoroutine(SpawnDungeon());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        upgradeManager.DisplayUpgrades(upgrades, player, ExitUpgrades);
    }
    void ExitUpgrades()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Player.Enable();
    }
}
