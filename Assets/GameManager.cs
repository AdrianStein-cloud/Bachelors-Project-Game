using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DungeonGenerator dungeonGenerator;
    GameObject player;
    ObjectiveSpawner objectiveSpawner;
    SimpleEnemySpawner enemySpawner;

    GameObject dungeon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dungeonGenerator = GetComponent<DungeonGenerator>();
        objectiveSpawner = GetComponent<ObjectiveSpawner>();
        enemySpawner = GetComponent<SimpleEnemySpawner>();
        StartCoroutine(StartRound());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        Destroy(dungeon);
        dungeon = new GameObject("Dungeon");
        yield return dungeonGenerator.GenerateDungeon(dungeon.transform);
        player.transform.position = dungeonGenerator.playerSpawnPosition.transform.position;
        enemySpawner.SpawnEnemies(dungeonGenerator.spawnedRooms, dungeon.transform);
        objectiveSpawner.SpawnObjectives(dungeonGenerator.spawnedRoomsDepth, dungeon.transform);
    }
}
