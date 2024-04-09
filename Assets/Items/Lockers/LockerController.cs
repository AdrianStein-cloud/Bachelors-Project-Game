using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockerController : Item
{
    [SerializeField] GameObject lockerPrefab;
    [SerializeField] GameObject lockerGhostPrefab;
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] LayerMask placeLayer;
    [SerializeField] float placeDistance;

    GameObject ghostLocker;
    List<MeshRenderer> lockerRenderers;
    bool isSelected;
    bool canPlace;
    int currentLockerCount;
    RaycastHit hit;
    GameObject player;

    [field: SerializeField] public int LockerCount { get; private set; }
    bool maximumPlaced => currentLockerCount == 0;

    List<GameObject> lockers;
    Collider[] overlapCols = new Collider[50];


    private void Awake()
    {
        ghostLocker = Instantiate(lockerGhostPrefab);

        ghostLocker.SetActive(false);
        lockerRenderers = ghostLocker.GetComponentsInChildren<MeshRenderer>().ToList();

        currentLockerCount = LockerCount;
        lockers = new();

        player = GameObject.FindGameObjectWithTag("Player");

        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.OnWaveOver += () =>
            {
                lockers = new();
                currentLockerCount = LockerCount;
                UpdateCounter();
            };
    }

    private void Start()
    {
        UpdateCounter();
    }

    private void Update()
    {
        if (!isSelected || maximumPlaced) return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, placeDistance, hitLayer))
        {
            var hitAmount = Physics.OverlapSphereNonAlloc(hit.point, 1, overlapCols, LayerMask.GetMask("NotPlaceable"));

            canPlace = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, placeDistance, placeLayer) && hitAmount <= 0;

            ghostLocker.SetActive(true);
            ghostLocker.transform.position = hit.point + new Vector3(0, 10, 0);
            var lockToPlayer = player.transform.position - ghostLocker.transform.position;
            lockToPlayer.y = 0;
            ghostLocker.transform.rotation = Quaternion.LookRotation(lockToPlayer, Vector3.up) * Quaternion.Euler(0, 90, 0);

            foreach(var renderer in lockerRenderers) renderer.material = canPlace ? validMaterial : invalidMaterial;
        }
        else
        {
            canPlace = false;
            ghostLocker.SetActive(false);
        }
    }

    public override void Primary() => TryPlace();

    private void UpdateCounter() => UnitySingleton<Inventory>.Instance.UpdateItemText(this, currentLockerCount.ToString());

    public override void Select()
    {
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
        canPlace = false;
        ghostLocker.SetActive(false);
    }

    private void TryPlace()
    {
        if (!canPlace || maximumPlaced) return;

        var instance = Instantiate(lockerPrefab, ghostLocker.transform.position, ghostLocker.transform.rotation);
        instance.transform.SetParent(hit.transform);


        instance.transform.Find("NotPlaceable").gameObject.SetActive(true);

        lockers.Add(instance);

        currentLockerCount--;

        UpdateCounter();

        if (maximumPlaced)
        {
            canPlace = false;
            ghostLocker.SetActive(false);
        }
    }
}
