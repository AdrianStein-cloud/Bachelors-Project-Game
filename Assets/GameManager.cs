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
    ElevatorButton elevator;
    DangerScaler dangerScaler;

    public Action<int> OnDungeonGenerated;
    public Action OnWaveOver;
    public Action OnDungeonEnter;

    GameObject dungeon;

    private void Awake()
    {
        UnitySingleton<GameManager>.BecomeSingleton(this);
    }

    private void Start()
    {
        //waitingRoomSpawnPoint = GameObject.Find("WaitingRoomSpawnPoint");
        player = GameObject.FindGameObjectWithTag("Player");
        dungeonGenerator = GetComponent<DungeonGenerator>();
        objectiveSpawner = GetComponent<ObjectiveSpawner>();
        enemySpawner = GetComponent<SimpleEnemySpawner>();
        upgradeManager = GetComponent<UpgradeManager>();
        elevator = FindAnyObjectByType<ElevatorButton>();
        elevator.EnterDungeon = StartLevel;
        //player.transform.position = waitingRoomSpawnPoint.transform.position;
        dangerScaler = new DangerScaler();
        StartCoroutine(SpawnDungeon());
    }

    IEnumerator SpawnDungeon()
    {
        elevator.DungeonIsAvailable = false;
        dungeon = new GameObject("Dungeon");
        dungeon.AddComponent<Dungeon>();
        dangerScaler.ScaleDanger();
        yield return dungeonGenerator.GenerateDungeon(dungeon, GameSettings.Instance.CurrentDepth);
        elevator.DungeonIsAvailable = true;
        SaveScore();
        OnDungeonGenerated?.Invoke(GameSettings.Instance.Wave);
    }

    void StartLevel()
    {
        enemySpawner.SpawnEnemies();
        objectiveSpawner.SpawnObjectives(dungeonGenerator.spawnedRoomsDepth, dungeon.transform, SwitchToUpgrades);
        OnDungeonEnter?.Invoke();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("player_score_" + GameSettings.Instance.DifficultyConfig.difficulty, GameSettings.Instance.Wave);

        int score = PlayerPrefs.GetInt("player_score_" + GameSettings.Instance.DifficultyConfig.difficulty);
        int currentHighscore = PlayerPrefs.GetInt("high_score_" + GameSettings.Instance.DifficultyConfig.difficulty);

        if (score > currentHighscore)
        {
            PlayerPrefs.SetInt("high_score_" + GameSettings.Instance.DifficultyConfig.difficulty, score);
        }
    }

    public void SpawnSingleEnemy()
    {
        enemySpawner.SpawnSingleEnemy();
    }

    public void GuaranteeFlood()
    {
        dungeonGenerator.GuaranteeFlood();
    }

    public void AddExtraTempEnemy(int amount)
    {
        enemySpawner.AddExtraTemporaryEnemies(amount);
    }

    void SwitchToUpgrades(int upgrades)
    {
        //player.SetActive(false);
        //player.transform.position = waitingRoomSpawnPoint.transform.position;
        //player.SetActive(true);
        Destroy(dungeon);
        OnWaveOver?.Invoke();
        StartCoroutine(SpawnDungeon());
        upgradeManager.RefreshUpgrades();
    }
}
