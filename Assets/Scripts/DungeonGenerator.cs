using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject sourceRooms;
    [Header("Generation Settings")]
    [SerializeField] int depth = 1;
    [SerializeField] int seed = -1; // -1 = random seed
    [SerializeField] int lookahead = 3;
    [SerializeField] NavMeshSurface navmeshSurface;

    private List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> spawnedRooms = new List<GameObject>();

    void Start()
    {
        SetSeed(seed);
        LoadRooms();
        StartCoroutine(GenerateDungeon());
    }
    public void SetSeed(int seed)
    {
        if (seed == -1)
        {
            seed = Random.Range(1000, 10000000);
        }
        Random.InitState(seed);
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

    

    private IEnumerator GenerateDungeon()
    {
        //instantiate first object in rooms
        GameObject entrance = Instantiate(rooms[0], new Vector3(0, 0, 0), transform.rotation);

        yield return StartCoroutine(SpawnRoomsAtDoorsCoroutine(entrance.GetComponent<Room>().GetDoors(), 0));
        
        //Done spawning dungeon
        yield return new WaitForSeconds(1f);
        navmeshSurface.BuildNavMesh();

        yield return new WaitForSeconds(1f);
        GetComponent<SimpleEnemySpawner>().SpawnEnemies(spawnedRooms);
    }

    IEnumerator SpawnRoomsAtDoorsCoroutine(List<Door> doors, int depth)
    {
        if (doors is null || doors.Count == 0 || depth >= this.depth) yield break;

        foreach (Door door in doors)
        {
            yield return StartCoroutine(SpawnRoomAtDoor(door, depth));
        }
    }

    IEnumerator SpawnRoomAtDoor(Door door, int depth)
    {
        bool roomFound = false;
        GameObject newRoom = null;
        door.debugHighlight = true;

        //Debug.Log("Room List: " + rooms.Count);

        List<GameObject> shuffledRooms = ShuffleList(rooms);

        //Debug.Log("Shuffled List: " + shuffledRooms.Count);

        foreach(GameObject room in shuffledRooms)
        {
            newRoom = Instantiate(room, door.gameObject.transform.position, door.direction);

            yield return new WaitForSeconds(0.05f);

            if (newRoom.GetComponent<Room>().isColliding == true)
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
                break;
            }
        }

        door.debugHighlight = false;

        if (roomFound)
        {
            yield return StartCoroutine(SpawnRoomsAtDoorsCoroutine(newRoom.GetComponent<Room>().GetDoors(), depth + 1));
        }
    }

    public List<GameObject> ShuffleList(List<GameObject> list)
    {
        List<GameObject> shuffled = new List<GameObject>();
        List<GameObject> temp = new List<GameObject>(list);
        int listCount = list.Count;

        for (int i = 1; i < listCount; i++)
        {
            int index = Random.Range(1, temp.Count - 1);
            shuffled.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return shuffled;
    }
}
