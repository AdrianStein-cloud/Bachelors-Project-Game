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

    GameObject dungeon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dungeonGenerator = GetComponent<DungeonGenerator>();
        objectiveSpawner = GetComponent<ObjectiveSpawner>();
        enemySpawner = GetComponent<SimpleEnemySpawner>();
        upgradeManager = GetComponent<UpgradeManager>();
        StartCoroutine(StartRound());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        dungeon = new GameObject("Dungeon");
        yield return dungeonGenerator.GenerateDungeon(dungeon.transform);
        player.transform.position = dungeonGenerator.playerSpawnPosition.transform.position;
        enemySpawner.SpawnEnemies(dungeonGenerator.spawnedRooms, dungeon.transform);
        objectiveSpawner.SpawnObjectives(dungeonGenerator.spawnedRoomsDepth, dungeon.transform, SwitchToUpgrades);
    }

    public void SwitchToUpgrades()
    {
        player.SetActive(false);
        Destroy(dungeon);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager.Player.Disable();
        upgradeManager.DisplayUpgrades(3, player, GoToNextLevel);
    }

    void GoToNextLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Player.Enable();
        player.SetActive(true);
        StartCoroutine(StartRound());
    }
}
