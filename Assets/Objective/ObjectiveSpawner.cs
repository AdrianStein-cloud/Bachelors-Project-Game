using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSpawner : MonoBehaviour
{
    public int objectiveAmount = 20;
    public int leaveThreshold = 5;
    public int maxObtainableObjectives = 12; 
    public GameObject collectablePrefab;
    public GameObject objectiveTracker;


    Image objectiveFill;

    private void Awake()
    {
        objectiveFill = GameObject.Find("ObjectiveBar").transform.Find("Fill").GetComponent<Image>();
    }

    public void SpawnObjectives(List<(GameObject room, int depth)> roomsDepth, Transform dungeon, Action<int> leave)
    {
        var tracker = Instantiate(objectiveTracker, dungeon).GetComponent<ObjectiveTracker>();
        tracker.Init(maxObtainableObjectives, leaveThreshold, objectiveFill, leave);

        var rooms = roomsDepth
            .Select(t => new { room = t.room.GetComponent<Room>(), depth = t.depth })
            .SelectMany(t => Enumerable.Range(0, t.depth).Select(_ => t.room)) //Weights
            .ToList();

        for (int i = objectiveAmount; i > 0 && rooms.Count > 0; i--)
        {
            var randomRoom = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            rooms.Remove(randomRoom);
            var spawnPostions = randomRoom.ObjectiveSpawnPositions;
            var spawnPositionObj = spawnPostions[UnityEngine.Random.Range(0, spawnPostions.Count)];
            spawnPostions.Remove(spawnPositionObj);
            var spawnPoint = spawnPositionObj.transform.position;
            var objectiveCollectable = Instantiate(collectablePrefab, spawnPoint, RandomRotation(), dungeon);
            objectiveCollectable.GetComponentInChildren<Collectable>().onCollect = tracker.ObjetiveCollected;
        }
    }

    Quaternion RandomRotation()
    {
        return Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
    }
}
