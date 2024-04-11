using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    public int multiplicationFrequency = 5;


    Image objectiveFill;

    System.Random random;
    

    private void Awake()
    {
        objectiveFill = GameObject.Find("ObjectiveBar").transform.Find("Fill").GetComponent<Image>();
        random = new System.Random();
    }

    public void SpawnObjectives(List<(GameObject room, int depth)> roomsDepth, Transform dungeon, Action<int> leave)
    {
        int objectiveAmount = (int)(this.objectiveAmount * (1 + GameSettings.Instance.Wave / (float)multiplicationFrequency));
        int leaveThreshold = (int)(this.leaveThreshold * (1 + GameSettings.Instance.Wave / (float)multiplicationFrequency));
        int maxObtainableObjectives = (int)(this.maxObtainableObjectives * (1 + GameSettings.Instance.Wave / (float)multiplicationFrequency));

        var tracker = Instantiate(objectiveTracker, dungeon).GetComponent<ObjectiveTracker>();
        Action giveCurrencyOnCollect = GetComponent<CurrencyManager>().OnObjectiveCollected;
        tracker.Init(maxObtainableObjectives, leaveThreshold, objectiveFill, giveCurrencyOnCollect, leave);

        var rooms = roomsDepth
            .Select(t => new Weighted<Room> { element = t.room.GetComponent<Room>(), Weight = t.depth}).ToList();

        //Debug.LogError("Dont TP me");

        for (int i = objectiveAmount; i > 0 && rooms.Count > 0; i--)
        {
            var randomRoom = rooms.GetRollFromWeights(random).element;
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

    void OnObjectiveCollected()
    {

    }

    Quaternion RandomRotation()
    {
        return Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
    }
}

public class Weighted<T> : IWeighted
{
    public T element;
    public int Weight { get; set; }
}
