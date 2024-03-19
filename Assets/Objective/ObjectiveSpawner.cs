using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSpawner : MonoBehaviour
{
    public int objectiveAmount = 20;
    public int leaveThreshold = 5;
    public int maxObtainableObjectives = 12; 
    public GameObject collectablePrefab;
    public GameObject objectiveTracker;

    public int multiplicationFrequancy = 5;


    Image objectiveFill;

    private void Awake()
    {
        objectiveFill = GameObject.Find("ObjectiveBar").transform.Find("Fill").GetComponent<Image>();
    }

    public void SpawnObjectives(List<(GameObject room, int depth)> roomsDepth, Transform dungeon, Action<int> leave)
    {
        int objectiveAmount = (int)(this.objectiveAmount * (1 + GameSettings.Instance.Wave / (float)multiplicationFrequancy));
        int leaveThreshold = (int)(this.leaveThreshold * (1 + GameSettings.Instance.Wave / (float)multiplicationFrequancy));
        int maxObtainableObjectives = (int)(this.maxObtainableObjectives * (1 + GameSettings.Instance.Wave / (float)multiplicationFrequancy));

        var tracker = Instantiate(objectiveTracker, dungeon).GetComponent<ObjectiveTracker>();
        tracker.Init(maxObtainableObjectives, leaveThreshold, objectiveFill, leave);
        

        var rooms = roomsDepth
            .Select(t => new { room = t.room.GetComponent<Room>(), depth = t.depth })
            .SelectMany(t => Enumerable.Range(0, t.depth).Select(_ => t.room)) //Weights
            .ToList();

        //Debug.LogError("Dont TP me");

        for (int i = objectiveAmount; i > 0 && rooms.Count > 0; i--)
        {
            var randomRoom = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            rooms.Remove(randomRoom);
            var spawnPostions = randomRoom.ObjectiveSpawnPositions;
            if (spawnPostions.Count == 0) {
                Debug.LogWarning("Room has no spawnpositions: " + randomRoom.name);
                i++;
                continue;
            }
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
