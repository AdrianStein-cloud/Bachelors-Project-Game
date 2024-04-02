using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public List<Room> Rooms { get; set; }

    public void Init(List<GameObject> rooms)
    {
        Rooms = rooms.Select(r => r.GetComponent<Room>()).ToList();
        UnitySingleton<Dungeon>.BecomeSingleton(this);
    }
}
