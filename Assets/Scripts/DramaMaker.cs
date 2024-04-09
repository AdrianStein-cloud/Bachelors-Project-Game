using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DramaMaker : Interactable
{
    bool inFocus = false;
    [SerializeField] TextMeshProUGUI computer;

    private List<WeightedDrama> dramaList;
    private WeightedDrama drama;
    private bool pressed = false;
    private AudioSource buttonSound;
    [SerializeField] private AudioSource typeSound;

    private static System.Random random;
    private GameManager gameManager;

    private void Awake()
    {
        UnitySingleton<GameManager>.Instance.OnDungeonGenerated += SetRandomDrama;
    }

    private void Start()
    {
        if (random == null) random = new System.Random(GameSettings.Instance.GetSeed());

        gameManager = FindObjectOfType<GameManager>();

        buttonSound = GetComponent<AudioSource>();
    }

    private void SetRandomDrama(int why)
    {
        dramaList = new List<WeightedDrama>
        {
            new WeightedDrama(ReleaseMonster, 100, 50, "Warning!\nAre you sure you want to release another monster?\n(bonus +50$)"),
            new WeightedDrama(ReleaseMonsterNextFloor, 100, 75, "Warning!\nRelease another monster on the next floor?\n(bonus +75$)"),
            new WeightedDrama(FloodNextFloor, 100, 50, $"Warning!\nAre you sure you want to open the floodgates on floor {GameSettings.Instance.Wave + 1}?\n(bonus +50$)"),
            new WeightedDrama(Nothing, 100, 0, "Are you sure you want to recharge all batteries?\n(penalty -50$)"),
            new WeightedDrama(Nothing, 100, 0, "ERROR 404: command not found"),
        };

        if (!GameSettings.Instance.PowerOutage)
        {
            dramaList.Add(new WeightedDrama(TurnOffLights, 100, 20, "Warning!\nAre you sure you want to turn off the power?\n(bonus +20$)"));
            dramaList.Add(new WeightedDrama(TurnOnLights, 100, -50, "Warning!\nAre you sure you want to turn on the backup generators?\n(penalty -50$)"));
        }

        if(GameSettings.Instance.Event == "Foggy!")
        {
            dramaList.Add(new WeightedDrama(TurnOffFog, 1000, -20, "Warning!\nAre you sure you want to turn on the ventilation system?\n(penalty -20$)"));
        }

        drama = dramaList.GetRollFromWeights(random);

        computer.text = drama.ToString();
    }

    private void TurnOffFog()
    {
        StartCoroutine(SlowWrite(computer, "Successfully Started Ventilation System."));
        GameSettings.Instance.Event = null;
        EventManager.Instance.ResetFog();
    }

    private void TurnOffLights()
    {
        StartCoroutine(SlowWrite(computer, "Power Off.\n\nGood Luck..."));
        GameSettings.Instance.PowerOutage = true;
    }

    private void TurnOnLights()
    {
        StartCoroutine(SlowWrite(computer, "Successfully Started Backup Generators."));
        GameSettings.Instance.PowerOnMode = true;
    }

    private void FloodNextFloor()
    {
        gameManager.GuaranteeFlood();
        StartCoroutine(SlowWrite(computer, $"Floodgates Successfully Opened on Floor {GameSettings.Instance.Wave + 1}."));
    }

    private void ReleaseMonster()
    {
        StartCoroutine(SlowWrite(computer, "Monster Released.\n\nGood Luck..."));
        gameManager.SpawnSingleEnemy();
    }

    private void ReleaseMonsterNextFloor()
    {
        StartCoroutine(SlowWrite(computer, $"Monster Successfully Released on Floor {GameSettings.Instance.Wave + 1}."));
        gameManager.AddExtraTempEnemy(1);
    }

    private void Nothing()
    {
        StartCoroutine(SlowWrite(computer, "ERROR...\nERROR...\nERROR...\nERROR...\nERROR...\nERROR..."));
    }

    private void PressButton()
    {
        if(drama == null)
        {
            SetRandomDrama(0);
            return;
        }
        InteractionUIText.Instance.SetText("");
        buttonSound.Play();
        transform.position -= new Vector3(0, 0.09f, 0);
        if(drama.bonus < 0 && gameManager.GetComponent<CurrencyManager>().Currency < (drama.bonus * -1))
        {
            StartCoroutine(ButtonWait());
            return;
        }
        gameManager.GetComponent<CurrencyManager>().AddCurrency(drama.bonus);
        pressed = true;
        drama.drama.Invoke();
    }

    IEnumerator ButtonWait()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position += new Vector3(0, 0.09f, 0);
    }

    IEnumerator SlowWrite(TextMeshProUGUI text, string content)
    {
        char[] chars = content.ToCharArray();
        string currentString = "";

        foreach (char c in chars)
        {
            yield return new WaitForSeconds(0.2f);
            currentString += c;
            typeSound.Play();
            text.text = currentString;
        }
    }

    private void Update()
    {
        if (InputManager.Player.Interact.triggered && inFocus && !pressed)
        {
            PressButton();
        }
        if (drama != null && drama.drama == TurnOffLights && GameSettings.Instance.PowerOutage)
        {
            SetRandomDrama(0);
        }
        if (drama != null && drama.drama == TurnOffFog && GameSettings.Instance.Event != "Foggy!")
        {
            SetRandomDrama(0);
        }
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        if (!pressed)
        {
            InteractionUIText.Instance.SetText("Press button");
        }
    }
}

public class WeightedDrama : IWeighted
{
    public WeightedDrama(Action drama, int Weight, int bonus, string description)
    {
        this.Weight = Weight;
        this.drama = drama;
        this.description = description;
        this.bonus = bonus;
    }

    public string description;
    public Action drama;
    public int bonus;
    [field: SerializeField] public int Weight { get; set; }

    public override string ToString()
    {
        return description;
    }
}
