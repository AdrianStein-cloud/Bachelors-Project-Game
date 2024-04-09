using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<GameObject> doors;
    List<Door> doorScripts;
    GameObject player;
    Renderer[] renderers;
    List<Door> Doors { 
        get
        {
            if(doorScripts == null || doorScripts.Count == 0)
            {
                doorScripts = doors.Select(d => d.GetComponent<Door>()).ToList();
            }
            return doorScripts;
        } 
    }


    Dictionary<Door, Room> adjecentRooms;
    public Dictionary<Door, Room> AdjecentRooms
    {
        get
        {
            if (adjecentRooms == null)
            {
                adjecentRooms = Doors
                    .Where(d => d.GetDoorConnected())
                    .ToDictionary(d => d, d => d.ConnectedDoor.transform.parent.GetComponent<Room>());
            }
            return adjecentRooms;
        }
    }

    Rigidbody rigidbody;
    private Door entrance;

    [Header("Bounding Box")]
    [SerializeField] public float bounding_x = 50;
    [SerializeField] public float bounding_y = 30;
    [SerializeField] public float bounding_z = 50;

    [Header("Offset")]
    [SerializeField] public float offset_x = 0;
    [SerializeField] public float offset_y = 0;
    [SerializeField] public float offset_z = 0;

    public GameObject centerObject;

    public bool isCorridor;

    public int depth = 0;

    public List<GameObject> ObjectiveSpawnPositions { get; private set; }
    public List<GameObject> KeySpawnPositions { get; private set; }
    public List<GameObject> ChestSpawnPositions { get; private set; }
    public List<GameObject> roamPositions;

    [Header("Random Objects")]
    [SerializeField] List<RandomObjects> randomObjects;
    [Header("Random Materials")]
    [SerializeField] List<GameObject> walls;
    [SerializeField] List<GameObject> floors;
    [SerializeField] List<GameObject> ceilings;

    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(bounding_x - 1, bounding_y, bounding_z - 1);
            boxCollider.center = new Vector3(0 + offset_x, bounding_y / 2 + offset_y, bounding_z / 2 + offset_z);
        }

        if(GetComponent<Rigidbody>() == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        entrance = Doors.First(d => d.isEntrance);

        //#if !UNITY_EDITOR
        UnitySingleton<GameManager>.Instance.OnDungeonGenerated += ReduceLag;
        //#endif
    }

    private void ReduceLag(int why)
    {
        StartCoroutine(ReduceLagCoroutine());
    }

    IEnumerator ReduceLagCoroutine()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        renderers = GetComponentsInChildren<Renderer>();

        float renderDistance = 400f;
        bool visible = true;

        while (enabled)
        {
            yield return new WaitForSeconds(1f);
            if(Vector3.Distance(player.transform.position, transform.position) >= renderDistance && visible)
            {
                foreach(Renderer renderer in renderers)
                {
                    if(renderer == null)
                    {
                        renderers = GetComponentsInChildren<Renderer>();
                        break;
                    }
                    renderer.enabled = false;
                }
                visible = false;
            }
            else if(Vector3.Distance(player.transform.position, transform.position) < renderDistance && !visible)
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer == null)
                    {
                        renderers = GetComponentsInChildren<Renderer>();
                        break;
                    }
                    renderer.enabled = true;
                }
                visible = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0 + offset_x, bounding_y/2 + offset_y, bounding_z / 2 + offset_z), new Vector3(bounding_x, bounding_y, bounding_z));
    }

    public bool IsPointWithinRoom(Vector3 point)
    {
        /*Vector3 center = boxCollider.center + transform.position;
        var test = new GameObject("CENTER");
        test.transform.position = center;
        Vector3 size = boxCollider.size / 2;

        bool x = point.x >= center.x - size.x & point.x <= center.x + size.x;
        bool y = point.y >= center.y - size.y & point.y <= center.y + size.y;
        bool z = point.z >= center.z - size.z & point.z <= center.z + size.z;
        return x & y & z;*/
        return boxCollider.bounds.Contains(point);
    }

    public Door GetEntrance()
    {
        return entrance;
    }

    public List<Door> GetDoors()
    {
        return Doors.Where(d => !d.isEntrance).ToList();
    }

    public IEnumerable<(Door, Room)> GetAdjecentRooms()
    {
        return Doors.Where(d => d.GetDoorConnected()).Select(d => (d, d.ConnectedDoor.transform.parent.GetComponent<Room>()));
    }

    public void InitRoom(System.Random random, Material wallMaterial, Material floorMaterial, Material ceilingMaterial)
    {
        SpawnRandomObjects(random);
        ApplyMaterials(wallMaterial, floorMaterial, ceilingMaterial);
    }

    private void ApplyMaterials(Material wallMaterial, Material floorMaterial, Material ceilingMaterial)
    {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<MeshRenderer>().material = wallMaterial;
        }
        foreach (GameObject floor in floors)
        {
            floor.GetComponent<MeshRenderer>().material = floorMaterial;
        }
        foreach (GameObject ceiling in ceilings)
        {
            ceiling.GetComponent<MeshRenderer>().material = ceilingMaterial;
        }
    }

    private void SpawnRandomObjects(System.Random random)
    {
        foreach (RandomObjects _randomObjects in randomObjects)
        {
            _randomObjects.randomObjects.Shuffle(random);

            for (int i = 0; i < _randomObjects.randomObjects.Count; i++)
            {
                _randomObjects.randomObjects[i].SetActive(false);
                if (i < _randomObjects.maxAmount && random.Next(1, 101) <= _randomObjects.percentageChance)
                {
                    _randomObjects.randomObjects[i].SetActive(true);
                }
            }

            //destroy all the objects that are not active
            for(int i = 0; i < _randomObjects.randomObjects.Count; i++)
            {
                if (!_randomObjects.randomObjects[i].activeSelf)
                {
                    Destroy(_randomObjects.randomObjects[i]);
                }
            }
        }

        ObjectiveSpawnPositions = GetComponentsInChildren<Transform>()
            .Where(t => t.name.Contains("ObjectiveSpawnPoint"))
            .Select(t => t.gameObject)
            .ToList();

        KeySpawnPositions = GetComponentsInChildren<Transform>()
            .Where(t => t.name.Contains("KeySpawnPoint"))
            .Select(t => t.gameObject)
            .ToList();

        ChestSpawnPositions = GetComponentsInChildren<Transform>()
            .Where(t => t.name.Contains("ChestSpawnPoint"))
            .Select(t => t.gameObject)
            .ToList();
    }

    [System.Serializable]
    public class RandomObjects
    {
        public string name;
        public int maxAmount;
        public int percentageChance;
        public List<GameObject> randomObjects;
    }
}
