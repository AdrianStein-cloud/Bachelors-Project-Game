using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public WeightedEnemy[] roundStartEnemies;

    public WeightedEnemy[] duringRoundEnemies;
    public float releaseGapMiniutes = 5;
    public int relesaeAmount = 2;

    public int extraEnemies;

    private System.Random random;

    private int tempExtraEnemies = 0;

    private void Start()
    {
        random = new System.Random(GameSettings.Instance.GetSeed());
    }

    public void SpawnEnemies()
    {
        int enemyAmount = GameSettings.Instance.EnemyAmount + extraEnemies + tempExtraEnemies;
        tempExtraEnemies = 0;
        ChooseAndSpawnEnemies(roundStartEnemies, enemyAmount);
        UnitySingleton<Dungeon>.Instance.AddComponent<EmptyScript>().StartCoroutine(SpawnAddtionalEnemiesAfterDelay());
    }

    public void SpawnSingleEnemy()
    {
        ChooseAndSpawnEnemies(roundStartEnemies, 1);
    }

    public void AddExtraTemporaryEnemies(int amount)
    {
        tempExtraEnemies += amount;
    }

    IEnumerator SpawnAddtionalEnemiesAfterDelay()
    {
        while (true)
        {
            float duration = releaseGapMiniutes * 60;
            UnitySingleton<SpawnTimerUIController>.Instance.StartTimer(duration);
            yield return new WaitForSeconds(duration);
            ChooseAndSpawnEnemies(duringRoundEnemies, relesaeAmount);
            break;
        }
    }

    void ChooseAndSpawnEnemies(IEnumerable<WeightedEnemy> enemies, int amount)
    {
        var dungeon = UnitySingleton<Dungeon>.Instance;
        var player = GameObject.FindGameObjectWithTag("Player");
        var chosenEnemies = ChooseEnemiesToSpawn(enemies, dungeon, amount);
        SpawnEnemies(chosenEnemies, dungeon, player);
    }

    IEnumerable<GameObject> ChooseEnemiesToSpawn(IEnumerable<WeightedEnemy> weightedEnemies, Dungeon dungeon, int amount)
    {
        var enemies = weightedEnemies.Where(e => e.MinWave <= dungeon.Wave).ToList();
        var enemiesToSpawn = enemies.ToDictionary(e => e, _ => 0);

        for (int i = 0; i < amount && enemies.Count > 0; i++)
        {
            var enemy = enemies.GetRollFromWeights(random);
            enemiesToSpawn[enemy]++;
            if (enemiesToSpawn[enemy] == enemy.MaxAmount)
            {
                enemies.Remove(enemy);
            }
        }
        return enemiesToSpawn.SelectMany(p => Enumerable.Range(0, p.Value).Select(_ => p.Key.enemyPrefab));
    }

    void SpawnEnemies(IEnumerable<GameObject> enemyPrefabs, Dungeon dungeon, GameObject player)
    {
        var enemies = enemyPrefabs.ToList();
        var rooms = dungeon.Rooms
            .OrderByDescending(r => Vector3.Distance(r.transform.position, player.transform.position))
            .ToList();

        for (int i = 0; i < rooms.Count & i < enemies.Count; i++)
        {
            Instantiate(enemies[i], rooms[i].centerObject.transform.position, Quaternion.identity, dungeon.transform);
        }
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
