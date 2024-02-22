using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject playerSpawnPosition { get; private set; }


    [Header("Generation Settings")]
    int depth = 1;
    [SerializeField] int seed = -1; // -1 = random seed
    [SerializeField] NavMeshSurface navmeshSurface;
    Random random;

    public WeightedRoom[] randomRooms;
    [field: SerializeField] public MinMaxRoom[] minimumRooms;
    public GameObject startRoom;
    private List<GameObject> rooms = new List<GameObject>();

    public List<GameObject> spawnedRooms { get; private set; } = new List<GameObject>();
    public List<(GameObject, int)> spawnedRoomsDepth = new List<(GameObject, int)>();
    private Queue<(Door, int)> doorQueue = new Queue<(Door, int)>();

    void Awake()
    {
        SetSeed(seed);
        LoadRooms();
        //StartCoroutine(GenerateDungeon());
        //RegenerateDungeon();
    }
    public void SetSeed(int seed)
    {
        if (seed == -1)
        {
            seed = new Random().Next(1000, 10000000);
        }
        random = new Random(seed);
        GameSettings.Instance.SetSeed(seed);
        //Debug.Log("Seed: " + seed);
    }

    private void LoadRooms()
    {
        //Debug.Log("There are " + rooms.Count + " different rooms...");

        //sourceRooms.SetActive(false);

        foreach (WeightedRoom room in randomRooms)
        {
            room.room.transform.position = Vector3.zero;
        }
    }

    public IEnumerator GenerateDungeon(Transform dungeon, int depth)
    {
        this.depth = depth;
        spawnedRooms = new List<GameObject>();
        spawnedRoomsDepth = new List<(GameObject, int)>();

        //instantiate first object in rooms
        GameObject entrance = Instantiate(startRoom, new Vector3(0, 0, 0), transform.rotation, dungeon);
        entrance.GetComponent<Room>().SpawnRandomObjects(random);
        playerSpawnPosition = entrance.transform.Find("PlayerSpawnPosition").gameObject;
        spawnedRooms.Add(entrance);

        foreach (Door door in entrance.GetComponent<Room>().GetDoors())
        {
            doorQueue.Enqueue((door, 0));
        }

        while(doorQueue.Count > 0)
        {
            (Door, int) element = doorQueue.Dequeue();
            yield return SpawnRoomAtDoor(element.Item1, element.Item2, dungeon);
        }
        
        //Done spawning dungeon
        yield return new WaitForSeconds(1f);
        Instantiate(navmeshSurface, dungeon).BuildNavMesh();

        yield return new WaitForSeconds(1f);
        //GetComponent<SimpleEnemySpawner>().SpawnEnemies(spawnedRooms);
        //GetComponent<ObjectiveSpawner>().SpawnObjectives(spawnedRoomsDepth);
    }

    IEnumerator SpawnRoomAtDoor(Door door, int depth, Transform dungeon)
    {
        bool roomFound = false;
        GameObject newRoom = null;
        door.debugHighlight = true;
        List<Door> doors = null;

        List<WeightedRoom> tempRandomRooms = new (randomRooms);

        while (!roomFound || tempRandomRooms.Count > 0)
        {
            WeightedRoom randomRoom;

            if (tempRandomRooms.Count < 1) break;

            if (tempRandomRooms.Count > 1) randomRoom = tempRandomRooms.GetRollFromWeights(random);
            else randomRoom = tempRandomRooms[0];

            newRoom = Instantiate(randomRoom.room, door.gameObject.transform.position, door.direction, dungeon);
            Room newRoomScript = newRoom.GetComponent<Room>();
            newRoomScript.depth = depth;
            doors = newRoomScript.GetDoors();

            yield return new WaitForSeconds(Time.deltaTime * 10);

            if (newRoomScript.isColliding || (doors.Count <= 1 && depth % GameSettings.Instance.GenerationLookahead != 0))
            {
                Destroy(newRoom);
                doors = null;
                tempRandomRooms.Remove(randomRoom);
            }
            else
            {
                door.SetDoorConnected(true);
                newRoom.GetComponent<Room>().GetEntrance().GetComponent<Door>().SetDoorConnected(true);
                newRoomScript.SpawnRandomObjects(random);
                spawnedRooms.Add(newRoom);
                spawnedRoomsDepth.Add((newRoom, depth));
                roomFound = true;
                break;
            }
        }

        door.debugHighlight = false;

        if (roomFound)
        {
            foreach (Door doorTemp in doors)
            {
                if(depth + 1 < this.depth) doorQueue.Enqueue((doorTemp, depth + 1));
                else yield return SpawnMinMaxRoomAtDoor(doorTemp, dungeon);
            }
        }

        yield break;
    }

    IEnumerator SpawnMinMaxRoomAtDoor(Door door, Transform dungeon)
    {
        bool roomFound = false;
        GameObject newRoom = null;
        door.debugHighlight = true;
        List<Door> doors = null;

        foreach (MinMaxRoom room in minimumRooms)
        {
            if(room.minmax.y < 1)
            {
                continue;
            }

            newRoom = Instantiate(room.room, 
                door.gameObject.transform.position, 
                door.direction, dungeon);
            Room newRoomSript = newRoom.GetComponent<Room>();
            doors = newRoomSript.GetDoors();
            

            yield return new WaitForSeconds(Time.deltaTime * 10);

            if (newRoomSript.isColliding)
            {
                Destroy(newRoom);
                newRoom = null;
                newRoomSript = null;
                doors = null;
            }
            else
            {
                door.SetDoorConnected(true);
                newRoomSript.GetEntrance().GetComponent<Door>().SetDoorConnected(true);
                newRoomSript.SpawnRandomObjects(random);

                spawnedRooms.Add(newRoom);
                spawnedRoomsDepth.Add((newRoom, depth));

                if(random.Next(0, 2) == 1) room.minmax -= new Vector2(1, 1);
                else room.minmax -= new Vector2(1, 2);

                roomFound = true;
                break;
            }
        }

        door.debugHighlight = false;

        if (roomFound)
        {
            yield break;
        }
    }
}

[System.Serializable]
public class WeightedRoom : IWeighted
{
    public GameObject room;
    [field: SerializeField] public int Weight { get; set; }
}

[System.Serializable]
public class MinMaxRoom
{
    public GameObject room;
    public Vector2 minmax;
}
