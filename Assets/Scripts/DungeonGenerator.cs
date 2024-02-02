using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject sourceRooms;
    [Header("Generation Settings")]
    [SerializeField] int generationTriesPerRoom = 30;
    [SerializeField] int depth = 1;
    [SerializeField] int seed = -1; // -1 = random seed

    private List<GameObject> rooms = new List<GameObject>();

    void Start()
    {
        GameSettings.Instance.SetSeed(seed);
        Random.InitState(seed);
        LoadRooms();
        GenerateDungeon();
    }

    private void LoadRooms()
    {
        // Get all the child GameObjects
        GameObject[] childObjects = sourceRooms.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();

        // Add the child GameObjects to the list
        rooms.AddRange(childObjects);

        Debug.Log("There are " + rooms.Count + " different rooms...");

        sourceRooms.SetActive(false);

        foreach (GameObject room in rooms)
        {
            room.transform.position = Vector3.zero;
        }
    }

    

    private void GenerateDungeon()
    {
        //instantiate first object in rooms
        GameObject entrance = Instantiate(rooms[0], new Vector3(0, 0, 0), transform.rotation);

        StartCoroutine(SpawnRoomsAtDoorsCoroutine(entrance.GetComponent<Room>().GetDoors(), 0));
    }

    IEnumerator SpawnRoomsAtDoorsCoroutine(List<Door> doors, int depth)
    {
        if (doors is null || doors.Count == 0 || depth >= this.depth) yield break;

        foreach (Door door in doors)
        {
            StartCoroutine(SpawnRoomAtDoor(door, depth));
        }
    }

    IEnumerator SpawnRoomAtDoor(Door door, int depth)
    {
        int tries = 0;

        bool roomFound = false;
        GameObject newRoom = null;
        door.debugHighlight = true;

        while (!roomFound && newRoom is null && !door.GetDoorConnected() && tries <= generationTriesPerRoom)
        {
            int roomIndexNumber = Random.Range(1, rooms.Count);
            newRoom = Instantiate(rooms[roomIndexNumber], door.gameObject.transform.position, door.direction);

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
            }

            tries++;
        }

        door.debugHighlight = false;

        if (tries <= generationTriesPerRoom)
        {
            yield return StartCoroutine(SpawnRoomsAtDoorsCoroutine(newRoom.GetComponent<Room>().GetDoors(), depth + 1));
        }
    }
}
