using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<GameObject> doors;

    Rigidbody rigidbody;
    private GameObject entrance;

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

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            BoxCollider detectionCollider = gameObject.AddComponent<BoxCollider>();
            detectionCollider.isTrigger = true;
            detectionCollider.size = new Vector3(bounding_x - 1, bounding_y, bounding_z - 1);
            detectionCollider.center = new Vector3(0 + offset_x, bounding_y / 2 + offset_y, bounding_z / 2 + offset_z);
        }

        if(GetComponent<Rigidbody>() == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        foreach (GameObject door in doors)
        {
            if (door.GetComponent<Door>().isEntrance)
            {
                entrance = door;
                break;
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

    public GameObject GetEntrance()
    {
        return entrance;
    }

    public List<Door> GetDoors()
    {
        List<Door> doorScripts = new List<Door>();

        foreach (GameObject door in doors)
        {
            Door doorScript = door.GetComponent<Door>();
            if (!doorScript.isEntrance)
            {
                doorScripts.Add(doorScript);
            }
        }

        return doorScripts;
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
