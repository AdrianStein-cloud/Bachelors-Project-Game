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

    private void Start()
    {
        if (random == null) random = new System.Random(GameSettings.Instance.GetSeed());

        gameManager = FindObjectOfType<GameManager>();

        dramaList = new List<WeightedDrama>
        {
            new WeightedDrama(ReleaseMonster, 100, 50, "Warning!\nAre you sure you want to release another monter?\n(bonus +50$)"),
            new WeightedDrama(Nothing, 100, 0, "Warning!\nYou're dead..."),
        };

        if (!GameSettings.Instance.PowerOutage)
        {
            dramaList.Add(new WeightedDrama(TurnOffLights, 100, 20, "Warning!\nAre you sure you want to turn off the power?\n(bonus +20$)"));
        }

        drama = dramaList.GetRollFromWeights(random);

        computer.text = drama.ToString();

        buttonSound = GetComponent<AudioSource>();

    }

    private void TurnOffLights()
    {
        StartCoroutine(SlowWrite(computer, "Power Off.\nGood Luck..."));
        GameSettings.Instance.PowerOutage = true;
    }
    private void ReleaseMonster()
    {
        StartCoroutine(SlowWrite(computer, "Monster Released.\nGood Luck..."));
        gameManager.SpawnSingleEnemy();
    }

    private void Nothing()
    {
        StartCoroutine(SlowWrite(computer, "ERROR...\nERROR...\nERROR...\nERROR...\nERROR...\nERROR..."));
    }

    private void PressButton()
    {
        buttonSound.Play();
        gameManager.GetComponent<CurrencyManager>().AddCurrency(drama.bonus);
        transform.position -= new Vector3(0, 0.09f, 0);
        pressed = true;
        drama.drama.Invoke();
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
    }

    public override void DisableInteractability()
    {
        inFocus = false;
    }

    public override void EnableInteractability()
    {
        inFocus = true;
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
