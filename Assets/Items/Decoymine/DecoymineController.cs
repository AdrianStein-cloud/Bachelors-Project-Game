using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoymineController : Item
{
    [SerializeField] GameObject minePrefab;
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] LayerMask placeLayer;
    [SerializeField] LayerMask activationLayer;
    [SerializeField] float placeDistance;
    [SerializeField] float maxActivationDistance;

    [field: SerializeField] public int MineCount { get; private set; }

    bool maximumPlaced => currentMineCount == 0;

    List<Decoymine> mines;

    RaycastHit hit;
    GameObject ghostMine;
    MeshRenderer mineRenderer;
    bool isSelected;
    bool canPlace;
    int currentMineCount;

    private void Awake()
    {
        ghostMine = Instantiate(minePrefab);
        ghostMine.SetActive(false);
        mineRenderer = ghostMine.GetComponentInChildren<MeshRenderer>();

        currentMineCount = MineCount;
        mines = new();

        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.OnWaveOver += () =>
            {
                currentMineCount = MineCount;
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
            ghostMine.SetActive(true);
            ghostMine.transform.position = hit.point;

            Rotate(ghostMine.transform);

            canPlace = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, placeDistance, placeLayer);
            mineRenderer.material = canPlace ? validMaterial : invalidMaterial;
        }
        else
        {
            canPlace = false;
            ghostMine.SetActive(false);
        }

    }

    void Rotate(Transform mine)
    {
        bool areParallel = Mathf.Approximately(Mathf.Abs(Vector3.Dot(Vector3.up, hit.normal)), 1f);
        Vector3 newForward = areParallel ? Vector3.right : Vector3.ProjectOnPlane(Vector3.up, hit.normal).normalized;
        mine.rotation = Quaternion.LookRotation(newForward, hit.normal);
    }

    public override void Primary() => TryPlace();

    public override void Secondary() => TryActivate();

    public override void Select()
    {
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
        canPlace = false;
        ghostMine.SetActive(false);
    }

    private void TryPlace()
    {
        if (!canPlace || maximumPlaced) return;

        var instance = Instantiate(minePrefab, hit.point, Quaternion.identity);
        Rotate(instance.transform);
        instance.transform.SetParent(hit.transform);

        var mine = instance.GetComponent<Decoymine>();
        mine.Init();
        mines.Add(mine);

        currentMineCount--;

        UpdateCounter();

        if (maximumPlaced)
        {
            canPlace = false;
            ghostMine.SetActive(false);
        }
    }

    private void TryActivate()
    {
        foreach (var mine in mines)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, maxActivationDistance, activationLayer))
            {
                if (hit.collider.gameObject == mine.gameObject)
                {
                    mine.Activate();
                    break;
                }
            }
        }
    }

    private void UpdateCounter() => UnitySingleton<Inventory>.Instance.UpdateItemText(this, currentMineCount.ToString());

    public void Upgrade(int count)
    {
        MineCount += count;
        currentMineCount = MineCount;
        UpdateCounter();
    }
}
