using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public int amountToSpawn;
    public GameObject enemyPrefab;

    public void SpawnEnemies(List<GameObject> rooms)
    { 
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject room = rooms[UnityEngine.Random.Range(0, rooms.Count)];

            Instantiate(enemyPrefab, room.transform.localPosition, Quaternion.identity);

            rooms.Remove(room);
        }
    }
}
