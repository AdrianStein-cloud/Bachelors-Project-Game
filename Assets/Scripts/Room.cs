using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<GameObject> doors;

    Rigidbody rigidbody;
    private GameObject entrance;

    [Header("Bounding Box")]
    [SerializeField] float bounding_x = 50;
    [SerializeField] float bounding_y = 30;
    [SerializeField] float bounding_z = 50;

    [Header("Offset")]
    [SerializeField] float offset_x = 0;
    [SerializeField] float offset_y = 0;
    [SerializeField] float offset_z = 0;

    public bool isColliding = false;

    public GameObject centerObject;

    public bool isCorridor;

    public int depth = 0;

    [Header("Random Objects")]
    [SerializeField] List<RandomObjects> randomObjects;

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

    public void SpawnRandomObjects(System.Random random)
    {
        if (randomObjects == null || randomObjects.Count < 1) return;

        foreach (RandomObjects _randomObjects in randomObjects)
        {
            if(random.Next(1, 101) <= _randomObjects.percentageChance)
            {
                _randomObjects.randomObjects[random.Next(0, _randomObjects.randomObjects.Count)].SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Room"))
        {
            isColliding = true;
        }
    }

    [System.Serializable]
    public class RandomObjects
    {
        public string name;
        public int percentageChance;
        public List<GameObject> randomObjects;
    }
}
