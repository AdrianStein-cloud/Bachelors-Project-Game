using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public List<Room> Rooms { get; private set; }
    public int Wave { get; private set; }

    public void Init(List<GameObject> rooms, int wave)
    {
        Wave = wave;
        Rooms = rooms.Select(r => r.GetComponent<Room>()).ToList();
        UnitySingleton<Dungeon>.BecomeSingleton(this);
    }

    public Room GetCurrentRoom(Vector3 position)
    {
        return Rooms.FirstOrDefault(r => r.IsPointWithinRoom(position));
    }

    private void OnDestroy()
    {
        UnitySingleton<Dungeon>.BecomeSingleton(null);
    }
}
