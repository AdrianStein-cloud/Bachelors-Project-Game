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
        StartCoroutine(SpawnDungeon());
    }

    IEnumerator SpawnDungeon()
    {
        dungeonEntrance.DungeonIsAvailable = false;
        dungeon = new GameObject("Dungeon");
        yield return dungeonGenerator.GenerateDungeon(dungeon.transform);
        dungeonEntrance.DungeonIsAvailable = true;
    }

    void StartLevel()
    {
        enemySpawner.SpawnEnemies(dungeonGenerator.spawnedRooms, dungeon.transform);
        objectiveSpawner.SpawnObjectives(dungeonGenerator.spawnedRoomsDepth, dungeon.transform, SwitchToUpgrades);
        player.SetActive(false);
        player.transform.position = dungeonGenerator.playerSpawnPosition.transform.position;
        player.SetActive(true);
    }

    public void SwitchToUpgrades()
    {
        InputManager.Player.Disable();
        player.SetActive(false);
        player.transform.position = waitingRoomSpawnPoint.transform.position;
        player.SetActive(true);
        Destroy(dungeon);
        StartCoroutine(SpawnDungeon());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        upgradeManager.DisplayUpgrades(3, player, ExitUpgrades);
    }
    void ExitUpgrades()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Player.Enable();
    }
}
