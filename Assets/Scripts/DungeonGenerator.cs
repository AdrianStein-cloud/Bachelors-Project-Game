using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject sourceRooms;
    public GameObject playerSpawnPosition;


    [Header("Generation Settings")]
    [SerializeField] int depth = 1;
    [SerializeField] int seed = -1; // -1 = random seed
    [SerializeField] int lookahead = 3;
    [SerializeField] NavMeshSurface navmeshSurface;
    Random random;

    private List<GameObject> rooms = new List<GameObject>();
    public List<GameObject> spawnedRooms = new List<GameObject>();
    public List<(GameObject, int)> spawnedRoomsDepth = new List<(GameObject, int)>();

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
        // Get all the child GameObjects
        GameObject[] childObjects = sourceRooms.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();

        // Add the child GameObjects to the list
        rooms.AddRange(childObjects);

        //Debug.Log("There are " + rooms.Count + " different rooms...");

        sourceRooms.SetActive(false);

        foreach (GameObject room in rooms)
        {
            room.transform.position = Vector3.zero;
        }
    }

    

    public IEnumerator GenerateDungeon(Transform dungeon)
    {
        spawnedRooms = new List<GameObject>();
        spawnedRoomsDepth = new List<(GameObject, int)>();

        //instantiate first object in rooms
        GameObject entrance = Instantiate(rooms[0], new Vector3(0, 0, 0), transform.rotation, dungeon);
        spawnedRooms.Add(entrance);

        yield return SpawnRoomsAtDoorsCoroutine(entrance.GetComponent<Room>().GetDoors(), 0, dungeon);
        
        //Done spawning dungeon
        yield return new WaitForSeconds(1f);
        navmeshSurface.BuildNavMesh();

        yield return new WaitForSeconds(1f);
        //GetComponent<SimpleEnemySpawner>().SpawnEnemies(spawnedRooms);
        //GetComponent<ObjectiveSpawner>().SpawnObjectives(spawnedRoomsDepth);
    }

    IEnumerator SpawnRoomsAtDoorsCoroutine(List<Door> doors, int depth, Transform dungeon)
    {
        if (doors is null || doors.Count == 0 || depth >= this.depth) yield break;

        foreach (Door door in doors)
        {
            yield return SpawnRoomAtDoor(door, depth, dungeon);
        }
    }

    IEnumerator SpawnRoomAtDoor(Door door, int depth, Transform dungeon)
    {
        bool roomFound = false;
        GameObject newRoom = null;
        door.debugHighlight = true;

        //Debug.Log("Room List: " + rooms.Count);

        List<GameObject> shuffledRooms = new (rooms);
        shuffledRooms.Shuffle();

        //Debug.Log("Shuffled List: " + shuffledRooms.Count);

        foreach(GameObject room in shuffledRooms)
        {
            newRoom = Instantiate(room, door.gameObject.transform.position, door.direction, dungeon);

            yield return new WaitForSeconds(Time.deltaTime * 10);

            if (newRoom.GetComponent<Room>().isColliding)
            {
                Destroy(newRoom);
                newRoom = null;
            }
            else
            {
                door.SetDoorConnected(true);
                newRoom.GetComponent<Room>().GetEntrance().GetComponent<Door>().SetDoorConnected(true);
                roomFound = true;
                spawnedRooms.Add(newRoom);
                spawnedRoomsDepth.Add((newRoom, depth));
                break;
            }
        }

        door.debugHighlight = false;

        if (roomFound)
        {
            yield return SpawnRoomsAtDoorsCoroutine(newRoom.GetComponent<Room>().GetDoors(), depth + 1, dungeon);
        }
    }
}
