using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public WeightedEnemy[] spawnPool;

    [Header("(Wave, Min, Min)")]
    public Vector3[] spawnTimes;

    public int extraEnemies;

    private System.Random random;

    private int tempExtraEnemies = 0;

    Dictionary<WeightedEnemy, int> currentPool;

    public CameraShakePreset shakePreset;
    public AudioSource spawnAudioSource;

    private void Start()
    {
        random = new System.Random(GameSettings.Instance.GetSeed());
    }

    public void SpawnEnemies()
    {
        currentPool = spawnPool.ToDictionary(e => e, _ => 0);
        int enemyAmount = GameSettings.Instance.EnemyAmount + extraEnemies + tempExtraEnemies;
        tempExtraEnemies = 0;
        ChooseAndSpawnEnemies(spawnPool, enemyAmount, r => r.depth);
        UnitySingleton<Dungeon>.Instance.AddComponent<EmptyScript>().StartCoroutine(SpawnAddtionalEnemiesAfterDelay());
    }

    public void SpawnSingleEnemy()
    {
        EnemySpawnEffect();
        var player = GameObject.FindGameObjectWithTag("Player");
        ChooseAndSpawnEnemies(spawnPool, 1, r => Vector3.Distance(r.transform.position, player.transform.position));
    }

    public void AddExtraTemporaryEnemies(int amount)
    {
        tempExtraEnemies += amount;
    }

    IEnumerator SpawnAddtionalEnemiesAfterDelay()
    {
        var dungeon = UnitySingleton<Dungeon>.Instance;
        var spawnTimeVector = spawnTimes.Where(t => t.x <= dungeon.Wave).MaxBy(t => t.x);
        var spawnTime = new List<float> { spawnTimeVector.y}; 
        if(spawnTimeVector.z != 0)
            spawnTime.Add(spawnTimeVector.z);

        for (int i = 0; i < spawnTime.Count; i++)
        {
            float wait = spawnTime[i] * 60;
            yield return new WaitForSeconds(wait);
            SpawnSingleEnemy();
            if (i == spawnTime.Count - 1) //Loop on last element
                i--;
        }
    }

    void ChooseAndSpawnEnemies(IEnumerable<WeightedEnemy> enemies, int amount, Func<Room, float> orderFunc)
    {
        var dungeon = UnitySingleton<Dungeon>.Instance;
        var chosenEnemies = ChooseEnemiesToSpawn(enemies, dungeon, amount);
        SpawnEnemies(chosenEnemies, dungeon, orderFunc);
    }

    IEnumerable<GameObject> ChooseEnemiesToSpawn(IEnumerable<WeightedEnemy> weightedEnemies, Dungeon dungeon, int amount)
    {
        var enemies = weightedEnemies.Where(e => e.MinWave <= dungeon.Wave & currentPool.ContainsKey(e)).ToList();
        var toSpawn = new List<GameObject>();

        for (int i = 0; i < amount && enemies.Count > 0; i++)
        {
            var enemy = enemies.GetRollFromWeights(random);
            toSpawn.Add(enemy.enemyPrefab);
            currentPool[enemy]++;
            if (currentPool[enemy] == enemy.MaxAmount)
            {
                enemies.Remove(enemy);
                currentPool.Remove(enemy);
            }
        }
        return toSpawn;
    }

    void SpawnEnemies(IEnumerable<GameObject> enemyPrefabs, Dungeon dungeon, Func<Room, float> orderFunc)
    {
        var enemies = enemyPrefabs.ToList();
        var rooms = dungeon.Rooms
            .OrderByDescending(orderFunc)
            .ToList();

        for (int i = 0; i < rooms.Count & i < enemies.Count; i++)
        {
            Instantiate(enemies[i], rooms[i].centerObject.transform.position, Quaternion.identity, dungeon.transform);
        }
    }

    void EnemySpawnEffect()
    {
        spawnAudioSource.Play();
        Camera.main.GetComponent<CameraShake>().Shake(shakePreset);
    }
}

[System.Serializable]
public class WeightedEnemy : IWeighted
{
    public string name;
    public GameObject enemyPrefab;
    [field: SerializeField] public int Weight { get; set; }
    public int MaxAmount;
    public int MinWave;
}
