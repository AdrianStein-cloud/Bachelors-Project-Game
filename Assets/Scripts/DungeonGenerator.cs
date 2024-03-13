using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
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
    public GameObject flood;
    private List<GameObject> rooms = new List<GameObject>();

    public List<GameObject> spawnedRooms { get; private set; } = new List<GameObject>();
    public List<(GameObject, int)> spawnedRoomsDepth = new List<(GameObject, int)>();
    private Queue<(Door, int)> doorQueue = new Queue<(Door, int)>();

    [SerializeField] private List<MaterialPackage> materialPackages;
    private Materials materials;

    private EventManager eventManager;

    void Awake()
    {
        LoadRooms();
        eventManager = new EventManager(flood);
    }
    public void SetSeed(int seed)
    {
        var newSeed = seed;
        if (seed == -1)
        {
            newSeed = new Random().Next(1000, 10000000);
        }
        LogSeed(newSeed);
        random = new Random(newSeed);
        GameSettings.Instance.SetSeed(newSeed);
    }

    private void LoadRooms()
    {
        foreach (WeightedRoom room in randomRooms)
        {
            room.room.transform.position = Vector3.zero;
        }
    }

    public IEnumerator GenerateDungeon(GameObject dungeon, int depth)
    {
        this.depth = depth;
        SetSeed(seed);
        RandomMaterialPackage(random);
        eventManager.SpawnRandomEvent(random);

        spawnedRooms = new List<GameObject>();
        spawnedRoomsDepth = new List<(GameObject, int)>();

        //instantiate first object in rooms
        GameObject entrance = Instantiate(startRoom, new Vector3(0, 0, 0), transform.rotation, dungeon.transform);
        entrance.GetComponent<Room>().InitRoom(random, materials.wall, materials.floor, materials.ceiling);
        playerSpawnPosition = entrance.transform.Find("PlayerSpawnPosition").gameObject;
        spawnedRooms.Add(entrance);

        foreach (Door door in entrance.GetComponent<Room>().GetDoors())
        {
            doorQueue.Enqueue((door, 0));
        }

        while (doorQueue.Count > 0)
        {
            (Door, int) element = doorQueue.Dequeue();
            yield return SpawnRoomAtDoor(element.Item1, element.Item2, dungeon.transform);
        }

        foreach(Door door in entrance.GetComponent<Room>().GetDoors())
        {
            if (!door.GetDoorConnected())
            {
                foreach (Transform child in dungeon.transform) Destroy(child.gameObject);
                yield return GenerateDungeon(dungeon, depth);
            }
        }

        //Done spawning dungeon
        yield return new WaitForSeconds(0.2f);
        var navMesh = Instantiate(navmeshSurface, dungeon.transform);
        navMesh.BuildNavMesh();
        eventManager.SetSizeOfDungeon(navMesh.navMeshData.sourceBounds.size);
        eventManager.SetCenterOfDungeon(navMesh.navMeshData.sourceBounds.center);


        yield return new WaitForSeconds(0.2f);
        //GetComponent<SimpleEnemySpawner>().SpawnEnemies(spawnedRooms);
        //GetComponent<ObjectiveSpawner>().SpawnObjectives(spawnedRoomsDepth);
    }

    IEnumerator SpawnRoomAtDoor(Door door, int depth, Transform dungeon)
    {
        bool roomFound = false;
        //GameObject newRoom = null;
        door.debugHighlight = true;
        List<Door> doors = null;

        List<WeightedRoom> tempRandomRooms = new(randomRooms);

        while (!roomFound || tempRandomRooms.Count > 0)
        {
            WeightedRoom randomRoom;

            if (tempRandomRooms.Count < 1) break;

            if (tempRandomRooms.Count > 1) randomRoom = tempRandomRooms.GetRollFromWeights(random);
            else randomRoom = tempRandomRooms[0];

            //newRoom = Instantiate(randomRoom.room, door.gameObject.transform.position, door.direction, dungeon);
            //newRoom = randomRoom.room.GetComponent<Room>;
            //Debug.Log("Trying to spawn: " + newRoom.gameObject.name);
            Room newRoomScript = randomRoom.room.GetComponent<Room>();
            newRoomScript.depth = depth;
            doors = newRoomScript.GetDoors();

            if (IsColliding(newRoomScript, door) || (doors.Count < 1 && (depth + 1) % GameSettings.Instance.GenerationLookahead != 0))
            {
                doors = null;
                tempRandomRooms.Remove(randomRoom);
            }
            else
            {
                var newRoom = Instantiate(randomRoom.room, door.gameObject.transform.position, door.direction, dungeon);
                newRoomScript = newRoom.GetComponent<Room>();
                doors = newRoomScript.GetDoors();
                door.SetDoorConnected(true);
                newRoomScript.GetEntrance().GetComponent<Door>().SetDoorConnected(true);
                newRoomScript.InitRoom(random, materials.wall, materials.floor, materials.ceiling);
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
                if (depth + 1 < this.depth) doorQueue.Enqueue((doorTemp, depth + 1));
                else yield return SpawnMinMaxRoomAtDoor(doorTemp, dungeon);
            }
        }

        yield return null;
    }

    IEnumerator SpawnMinMaxRoomAtDoor(Door door, Transform dungeon)
    {
        bool roomFound = false;
        //GameObject newRoom = null;
        door.debugHighlight = true;
        List<Door> doors = null;

        foreach (MinMaxRoom room in minimumRooms)
        {
            if (room.minmax.y < 1)
            {
                continue;
            }

            //newRoom = Instantiate(room.room, door.gameObject.transform.position, door.direction, dungeon);
            //newRoom = room.room;
            Room newRoomScript = room.room.GetComponent<Room>();
            doors = newRoomScript.GetDoors();


            if (IsColliding(newRoomScript, door))
            {
                //Destroy(newRoom);
                //newRoom = null;
                newRoomScript = null;
                doors = null;
            }
            else
            {
                var newRoom = Instantiate(room.room, door.gameObject.transform.position, door.direction, dungeon);
                newRoomScript = newRoom.GetComponent<Room>();
                doors = newRoomScript.GetDoors();
                door.SetDoorConnected(true);
                newRoomScript.GetEntrance().GetComponent<Door>().SetDoorConnected(true);
                newRoomScript.InitRoom(random, materials.wall, materials.floor, materials.ceiling);

                spawnedRooms.Add(newRoom);
                spawnedRoomsDepth.Add((newRoom, depth));

                if (random.Next(0, 2) == 1) room.minmax -= new Vector2(1, 1);
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

    void LogSeed(int seed)
    {
        string path = Application.dataPath + "/../Logs/Seeds.log";

        string folderPath = Application.dataPath + "/../Logs";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd\tHH:mm:ss") + "\t" + seed + "\t" + depth);
            writer.Flush();
        }
    }

    bool IsColliding(Room newRoomScript, Door door)
    {
        Vector3 roomSize = new Vector3(newRoomScript.bounding_x - 1f, newRoomScript.bounding_y, newRoomScript.bounding_z - 1f);
        Vector3 offset = new Vector3(newRoomScript.offset_x, newRoomScript.offset_y, newRoomScript.offset_z);


        var offsetCalculated = door.transform.forward * offset.z + door.transform.right * offset.x + door.transform.up * offset.y;
        var roomCenter = door.transform.position + (door.transform.forward * newRoomScript.bounding_z / 2) + new Vector3(0, newRoomScript.bounding_y / 2, 0) + offsetCalculated;
        var colliders = Physics.OverlapBox(roomCenter, roomSize / 2, door.transform.rotation, LayerMask.GetMask("ExcludeVision"));
        bool isColliding = false;

        /*
        Debug.Log("Name: " + newRoomScript.gameObject.name);
        Debug.Log("roomSize: " + roomSize);
        Debug.Log("roomCenter: " + roomCenter);
        */

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Room") && collider.gameObject != door.transform.parent.gameObject && collider.gameObject != newRoomScript.gameObject)
            {
                //Debug.Log("Cant spawn. This: " + newRoomScript.name + "\nCollided with: " + collider.gameObject.name);
                isColliding = true;
            }
        }

        return isColliding;
    }

    private void RandomMaterialPackage(System.Random random)
    {
        if(materialPackages.Count > 0)
        {
            MaterialPackage _materialPackage = materialPackages[random.Next(materialPackages.Count)];
            Material floor = _materialPackage.floorMaterials[random.Next(_materialPackage.floorMaterials.Count)];
            Material wall = _materialPackage.wallMaterials[random.Next(_materialPackage.wallMaterials.Count)];
            Material ceiling;
            if (_materialPackage.wallForCeiling)
            {
                ceiling = wall;
            }
            else
            {
                ceiling = _materialPackage.ceilingMaterials[random.Next(_materialPackage.ceilingMaterials.Count)];
            }
            materials = new Materials(floor, wall, ceiling);
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

[System.Serializable]
public class MaterialPackage
{
    public string name;
    public bool wallForCeiling;
    public List<Material> floorMaterials;
    public List<Material> wallMaterials;
    public List<Material> ceilingMaterials;
}

[System.Serializable]
public class Materials
{
    public Materials(Material floor, Material wall, Material ceiling)
    {
        this.floor = floor;
        this.wall = wall;
        this.ceiling = ceiling;
    }

    public Material floor;
    public Material wall;
    public Material ceiling;
}
