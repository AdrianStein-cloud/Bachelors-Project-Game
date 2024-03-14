using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

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

            Instantiate(enemyPrefab, room.transform.localPosition, Quaternion.identity, dungeon);

            rooms.Remove(room);
        }
    }
}
