using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public List<WeightedEnemy> enemies;

    public int extraEnemies;

    private System.Random random;

    private int tempExtraEnemies = 0;

    private void Start()
    {
        random = new System.Random(GameSettings.Instance.GetSeed());
    }

    public void SpawnEnemies(List<GameObject> rooms, Transform dungeon)
    {
        rooms = rooms.OrderByDescending(r => r.GetComponent<Room>().depth).ToList();
        for (int i = 0; i < GameSettings.Instance.EnemyAmount + extraEnemies + tempExtraEnemies; i++)
        {
            GameObject room = rooms[i];

            var enemy = enemies.GetRollFromWeights(random);

            Instantiate(enemy.enemyPrefab, room.transform.localPosition, Quaternion.identity, dungeon);
        }
        tempExtraEnemies = 0;
    }

    public void SpawnSingleEnemy(List<GameObject> rooms, Transform dungeon)
    {
        var room = rooms.OrderByDescending(r => r.GetComponent<Room>().depth).First();

        var enemy = enemies.GetRollFromWeights(random);

        Instantiate(enemy.enemyPrefab, room.transform.localPosition, Quaternion.identity, dungeon);
    }

    public void AddExtraTemporaryEnemies(int amount)
    {
        tempExtraEnemies += amount;
    }
}

[System.Serializable]
public class WeightedEnemy : IWeighted
{
    public string name;
    public GameObject enemyPrefab;
    [field: SerializeField] public int Weight { get; set; }
}
