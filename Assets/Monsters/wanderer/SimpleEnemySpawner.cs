using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public List<WeightedEnemy> enemies;

    public int extraEnemies;

    public void SpawnEnemies(List<GameObject> rooms, Transform dungeon, int depth)
    {
        rooms = new List<GameObject>(rooms);
        for (int i = 0; i < GameSettings.Instance.EnemyAmount + extraEnemies; i++)
        {
            GameObject room = rooms[UnityEngine.Random.Range(0, rooms.Count)];

            while (room.GetComponent<Room>().depth != depth)
            {
                room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            }

            var enemy = enemies.GetRollFromWeights(new System.Random(GameSettings.Instance.GetSeed()));

            Instantiate(enemy.enemyPrefab, room.transform.localPosition, Quaternion.identity, dungeon);

            rooms.Remove(room);
        }
    }

    public void SpawnSingleEnemy(List<GameObject> rooms, Transform dungeon, int depth)
    {
        rooms = new List<GameObject>(rooms);
        GameObject room = rooms[UnityEngine.Random.Range(0, rooms.Count)];

        while (room.GetComponent<Room>().depth != depth)
        {
            room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
        }

        var enemy = enemies.GetRollFromWeights(new System.Random(GameSettings.Instance.GetSeed()));

        Instantiate(enemy.enemyPrefab, room.transform.localPosition, Quaternion.identity, dungeon);

        rooms.Remove(room);
    }
}

[System.Serializable]
public class WeightedEnemy : IWeighted
{
    public string name;
    public GameObject enemyPrefab;
    [field: SerializeField] public int Weight { get; set; }
}
