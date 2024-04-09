using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using UnityEngine;
using System.Linq;
using Random = System.Random;
using UniversalForwardPlusVolumetric;
using BBUnity.Actions;
using UnityEngine.Rendering;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject playerSpawnPosition { get; private set; }


    [Header("Generation Settings")]
    int depth = 1;
    [SerializeField] int seed = -1; // -1 = random seed
    [SerializeField] NavMeshSurface navmeshSurface;
    Random random;

    public GameObject floorPrefab;

    public WeightedRoom[] randomRooms;
    [field: SerializeField] public List<WeightedEndRoom> endRooms;
    public GameObject startRoom;
    public GameObject flood;
    public VolumetricConfig volumetricConfig;

    public List<GameObject> spawnedRooms { get; private set; } = new List<GameObject>();
    public List<(GameObject, int)> spawnedRoomsDepth = new List<(GameObject, int)>();
    private Queue<(Door, int)> doorQueue = new Queue<(Door, int)>();

    List<(Door, Door)> potentialConnections = new List<(Door, Door)>();

    [SerializeField] private List<MaterialPackage> materialPackages;
    private Materials materials;

    private EventManager eventManager;

    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject chestPrefab;

    void Awake()
    {
        LoadRooms();
        eventManager = gameObject.AddComponent<EventManager>();
        eventManager.Init(flood, volumetricConfig);
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

        spawnedRooms = new List<GameObject>();
        spawnedRoomsDepth = new List<(GameObject, int)>();
        potentialConnections = new List<(Door, Door)>();
        doorQueue = new Queue<(Door, int)>();

        //instantiate first object in rooms
        GameObject entrance = Instantiate(startRoom, Vector3.zero, Quaternion.identity, dungeon.transform);
        entrance.GetComponent<Room>().InitRoom(random, materials.wall, materials.floor, materials.ceiling);
        playerSpawnPosition = entrance.transform.Find("PlayerSpawnPosition").gameObject;
        spawnedRooms.Add(entrance);

        bool dungeonFailed = false;

        foreach (Door door in entrance.GetComponent<Room>().GetDoors())
        {
            doorQueue.Enqueue((door, 0));
        }

        Debug.Log(doorQueue.Count);

        while (doorQueue.Count > 0)
        {
            if(spawnedRooms.Count == 4)
            {
                foreach (Door door in entrance.GetComponent<Room>().GetDoors())
                {
                    if (!door.GetDoorConnected())
                    {
                        StopAllCoroutines();
                        foreach (Transform child in dungeon.transform) Destroy(child.gameObject);
                        dungeonFailed = true;
                        StartCoroutine(GenerateDungeon(dungeon, depth));
                        yield break;
                    }
                }
            }
            (Door, int) element = doorQueue.Dequeue();
            yield return SpawnRoomAtDoor(element.Item1, element.Item2, dungeon.transform);
        }

        potentialConnections
            .Where(p => p.Item1 != null & p.Item2 != null)
            .Where(p => !p.Item1.GetDoorConnected() & !p.Item2.GetDoorConnected())
            .Where(p => p.Item1.transform.position.y == p.Item2.transform.position.y)
            .Where(p => Mathf.Approximately(Quaternion.Angle(p.Item1.direction, p.Item2.direction), 180))
            .OrderBy(p => Vector3.Distance(p.Item1.transform.position, p.Item2.transform.position))
            .ToList()
            .ForEach(p =>
            {
                if (!p.Item1.GetDoorConnected() & !p.Item2.GetDoorConnected())
                    SpawnLoopConnection(p.Item1, p.Item2, dungeon.transform);
            });
        

        if (!dungeonFailed)
        {
            eventManager.SpawnRandomEvent(random);
            RemoveUnecessaryWalls();


            Stats.Instance.player.keysHeld = 0;
            SpawnChestWithKeys();
            dungeon.GetComponent<Dungeon>().Init(spawnedRooms);

            //Done spawning dungeon
            yield return new WaitForSeconds(0.2f);
            var navMesh = Instantiate(navmeshSurface, dungeon.transform);
            navMesh.BuildNavMesh();
            eventManager.SetSizeOfDungeon(navMesh.navMeshData.sourceBounds.size);
            eventManager.SetCenterOfDungeon(navMesh.navMeshData.sourceBounds.center);
        }
    }

    IEnumerator SpawnRoomAtDoor(Door door, int depth, Transform dungeon)
    {
        bool roomFound = false;
        bool isCorridor = false;
        //GameObject newRoom = null;
        door.debugHighlight = true;
        List<Door> doors = null;

        List<WeightedRoom> tempRandomRooms = new(randomRooms);

        while (tempRandomRooms.Count > 0)
        {
            WeightedRoom randomRoom;

            if (tempRandomRooms.Count > 1) randomRoom = tempRandomRooms.GetRollFromWeights(random);
            else randomRoom = tempRandomRooms[0];

            //newRoom = Instantiate(randomRoom.room, door.gameObject.transform.position, door.direction, dungeon);
            //newRoom = randomRoom.room.GetComponent<Room>;
            //Debug.Log("Trying to spawn: " + newRoom.gameObject.name);
            Room newRoomScript = randomRoom.room.GetComponent<Room>();
            newRoomScript.depth = depth;
            doors = newRoomScript.GetDoors();

            if (IsColliding(newRoomScript, door, out Door potentialConnection) || (doors.Count == 0 && (depth + 1) % GameSettings.Instance.GenerationLookahead != 0))
            {
                potentialConnections.Add((door, potentialConnection));
                doors = null;
                tempRandomRooms.Remove(randomRoom);
            }
            else
            {
                var newRoom = Instantiate(randomRoom.room, door.gameObject.transform.position, door.direction, dungeon);
                newRoomScript = newRoom.GetComponent<Room>();
                doors = newRoomScript.GetDoors();
                var newEntrance = newRoomScript.GetEntrance();
                door.ConnectDoor(newEntrance);
                newEntrance.ConnectDoor(door);
                newRoomScript.InitRoom(random, materials.wall, materials.floor, materials.ceiling);
                spawnedRooms.Add(newRoom);
                spawnedRoomsDepth.Add((newRoom, depth));
                roomFound = true;
                if(newRoomScript.isCorridor) isCorridor = true;
                break;
            }
        }

        door.debugHighlight = false;

        if (roomFound)
        {
            foreach (Door doorTemp in doors)
            {
                if (depth + 1 < this.depth) doorQueue.Enqueue((doorTemp, depth + 1));
                else yield return SpawnEndRoomAtDoor(doorTemp, dungeon, isCorridor);
            }
        }

        yield return null;
    }

    IEnumerator SpawnEndRoomAtDoor(Door door, Transform dungeon, bool isCorridor)
    {
        bool roomFound = false;
        door.debugHighlight = true;

        List<WeightedEndRoom> tempRandomRooms = new(endRooms.Where(x => (x.useMaxAmount && x.maxAmount > 0) || !x.useMaxAmount));

        if (!isCorridor)
        {
            int chance = 40;
            if(random.Next(101) > chance)
            {
                yield break;
            }

            tempRandomRooms = tempRandomRooms.Where(room => !room.corridorOnly).ToList();
        }

        while(tempRandomRooms.Count > 0)
        {
            WeightedEndRoom randomRoom;

            if (tempRandomRooms.Count < 1) break;

            if (tempRandomRooms.Count > 1) randomRoom = tempRandomRooms.GetRollFromWeights(random);
            else randomRoom = tempRandomRooms[0];

            Room newRoomScript = randomRoom.room.GetComponent<Room>();


            if (IsColliding(newRoomScript, door, out Door potentialConnection))
            {
                potentialConnections.Add((door, potentialConnection));
                tempRandomRooms.Remove(randomRoom);
            }
            else
            {
                var newRoom = Instantiate(randomRoom.room, door.gameObject.transform.position, door.direction, dungeon);

                if (randomRoom.useMaxAmount)
                {
                    randomRoom.maxAmount--;
                }

                newRoomScript = newRoom.GetComponent<Room>();
                var newEntrance = newRoomScript.GetEntrance();
                door.ConnectDoor(newEntrance);
                newEntrance.ConnectDoor(door);
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

    bool IsColliding(Room newRoomScript, Door door, out Door potentialLoop)
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

        potentialLoop = null;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Room") && collider.gameObject != door.transform.parent.gameObject && collider.gameObject != newRoomScript.gameObject)
            {
                var collidingRoomScript = collider.GetComponent<Room>();
                if (collidingRoomScript == null) continue;
                var collidingRoomDoors = collidingRoomScript.GetDoors();
                var closetDoor = collidingRoomDoors
                    .Where(d => !d.GetDoorConnected())
                    .OrderBy(d => Vector3.Distance(door.transform.position, d.transform.position))
                    .FirstOrDefault();
                potentialLoop = closetDoor;

                //Debug.Log("Cant spawn. This: " + newRoomScript.name + "\nCollided with: " + collider.gameObject.name);
                isColliding = true;
            }
        }

        return isColliding;
    }

    void SpawnLoopConnection(Door from, Door to, Transform dungeon)
    {
        var dir = to.transform.position - from.transform.position;
        var center = (to.transform.position + from.transform.position) / 2;
        var halfExtents = new Vector3(Mathf.Abs(dir.x)/2, 23, Mathf.Abs(dir.z)/2);

        var colliders = Physics.OverlapBox(center, halfExtents, from.direction, LayerMask.GetMask("ExcludeVision"));
        bool connectable = colliders.FirstOrDefault(c => c.CompareTag("Room") & c.gameObject != from.transform.parent.gameObject & c.gameObject != to.transform.parent.gameObject) == null;
        bool codirectional = Mathf.Approximately(Vector3.Dot(Vector3.Project(dir, from.transform.forward), from.transform.forward), 1);

        if (!connectable || codirectional || Vector3.Dot(dir, from.transform.forward) < 10f) return;
        EnableCorridorOpeningIfCorridor(from);
        EnableCorridorOpeningIfCorridor(to);

        from.ConnectDoor(to);
        to.ConnectDoor(from);

        var floor = Instantiate(floorPrefab, from.transform.position, from.direction, dungeon);
        var scaler = floor.GetComponent<ConnectionRoom>();
        scaler.ApplyMaterials(materials.wall, materials.floor, materials.ceiling);

        var z = Vector3.Project(dir, floor.transform.forward);
        var x = Vector3.Project(dir, floor.transform.right);
        float dot = Vector3.Dot(x.normalized, floor.transform.right);
        bool sign = Mathf.Approximately(dot, -1);
        Vector2 diff = new Vector2((sign ? -1 : 1) * x.magnitude, z.magnitude);
        Vector2 scale = new Vector2(Mathf.Abs(diff.x), diff.y);
        floor.transform.position += floor.transform.right * diff.x / 2;
        scale.x += 12;
        scaler.Scale(scale, diff);
        //new GameObject($"({diff.x}, {diff.y})");
    }

    void EnableCorridorOpeningIfCorridor(Door door)
    {
        var room = door.transform.parent.GetComponent<Room>();
        if (!room.isCorridor || room.name.Contains("Entrance")) return;

        /*room.transform
            .Cast<Transform>()
            .ToList().ForEach(t => Debug.Log(t.name));*/


        var corridorOpening = room.transform
            .Cast<Transform>()
            .Where(t => t.name.Contains("CorridorWallOpening"))
            .OrderBy(t => Vector3.Distance(door.transform.position, t.position))
            .First();

        corridorOpening.gameObject.SetActive(true);
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

    private void OnDestroy()
    {
        eventManager.ResetFog();
    }

    private void SpawnChestWithKeys()
    {
        var chests = new List<GameObject>();
        chests.Add(SpawnChest());
        if (chests != null && chests[0] != null)
        {
            if (!SpawnKey())
            {
                Destroy(chests[0]);
            }
            else
            {
                for (int i = 0; i < Mathf.Floor(GameSettings.Instance.Wave / 3); i++)
                {
                    chests.Add(SpawnChest());
                }
                for (int i = 0; i < Mathf.Floor((chests.Count - 1) / 2); i++)
                {
                    SpawnKey();
                }
            }
        }
    }

    private bool SpawnKey()
    {
        var roomsWithKeySpawnPoints = spawnedRooms.Where(x => x.GetComponent<Room>().KeySpawnPositions.Count > 0).ToList();

        if(roomsWithKeySpawnPoints != null && roomsWithKeySpawnPoints.Count > 0)
        {
            var randomRoom = roomsWithKeySpawnPoints[random.Next(roomsWithKeySpawnPoints.Count)];
            var keySpawnPositions = randomRoom.GetComponent<Room>().KeySpawnPositions;
            var keySpawnPoint = keySpawnPositions[random.Next(keySpawnPositions.Count)];

            Instantiate(keyPrefab, keySpawnPoint.transform.position, keySpawnPoint.transform.rotation, randomRoom.transform);

            keySpawnPositions.Remove(keySpawnPoint);

            return true;
        }
        return false;
    }

    private GameObject SpawnChest()
    {
        var roomsWithChestSpawnPoints = spawnedRooms.Where(x => x.GetComponent<Room>().ChestSpawnPositions.Count > 0).ToList();

        if (roomsWithChestSpawnPoints != null && roomsWithChestSpawnPoints.Count > 0)
        {
            var randomRoom = roomsWithChestSpawnPoints[random.Next(roomsWithChestSpawnPoints.Count)];
            var chestSpawnPositions = randomRoom.GetComponent<Room>().ChestSpawnPositions;
            var chestSpawnPoint = chestSpawnPositions[random.Next(chestSpawnPositions.Count)];

            if(chestSpawnPoint != null)
            {
                var chest = Instantiate(chestPrefab, chestSpawnPoint.transform.position, chestSpawnPoint.transform.rotation, randomRoom.transform);

                chestSpawnPositions.Remove(chestSpawnPoint);

                return chest;
            }
        }
        return null;
    }

    public void GuaranteeFlood()
    {
        if(eventManager != null)
        {
            eventManager.GuaranteeFlood();
        }
    }

    void RemoveUnecessaryWalls()
    {
        /*spawnedRooms
            .Select(r => r.GetComponent<Room>().GetDoors())
            .SelectMany(doors => doors)
            .GroupBy(v => v.transform.position.RoundToNearestInt(), new VectorComparer())
            .ToList().ForEach(g =>
            {
                if (g.Count() > 1)
                {
                    g.ToList().ForEach(d => d.SetDoorConnected(true));
                }
            });*/

        spawnedRooms
                .Select(r => r.GetComponent<Room>().GetDoors())
                .SelectMany(doors => doors)
                .Where(d => !d.GetDoorConnected())
                .GetPairs(DoorsAreConnectable)
                .ToList()
                .ForEach(t =>
                {
                    t.Item1.ConnectDoor(t.Item2);
                    t.Item2.ConnectDoor(t.Item1);
                });


        bool DoorsAreConnectable(Door a, Door b)
        {
            Vector3 diff = b.transform.position - a.transform.position;
            bool opposite = a.transform.forward.Opposite(b.transform.forward);
            float forwardDistance = diff.DistanceAlongDirection(a.direction * Vector3.forward);
            float sidewaysDistance = diff.DistanceAlongDirection(a.direction * Vector3.right);
            bool forwardClose = Mathf.Abs(forwardDistance) < 0.1f;
            bool sidewaysDoable = Mathf.Abs(sidewaysDistance) < 11f && (a.GetComponentInParent<Room>().isCorridor || b.GetComponentInParent<Room>().isCorridor);
            bool sidewaysClose = Mathf.Abs(sidewaysDistance) < 0.1f;
            return opposite & forwardClose & (sidewaysClose | sidewaysDoable);
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
public class WeightedEndRoom : IWeighted
{
    public bool corridorOnly = true;
    public GameObject room;
    public bool useMaxAmount;
    public int maxAmount;
    [field: SerializeField] public int Weight { get; set; }
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
