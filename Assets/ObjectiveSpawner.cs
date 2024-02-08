using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveSpawner : MonoBehaviour
{
    public int objectiveAmount = 3;
    public GameObject collectablePrefab;

    public void SpawnObjectives(List<(GameObject room, int depth)> roomsDepth, Transform dungeon)
    {
        //This is very wack
        
        var rooms = roomsDepth.OrderByDescending(t => t.depth).Select(t => t.room);

        int i = objectiveAmount;
        foreach (var room in rooms)
        {
            var pos = room.transform.position + room.transform.forward * 20;
            var collectable = Instantiate(collectablePrefab, pos, Quaternion.identity, dungeon);
            collectable.GetComponent<Collectable>().onCollect = ObjetiveCollected;
            i--;
            if (i == 0) break;
        }
    }

    void ObjetiveCollected()
    {
        Debug.LogWarning("Collection event not yet implemented");
    }
}
