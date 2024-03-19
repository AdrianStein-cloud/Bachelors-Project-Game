using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DramaMaker : Interactable
{
    bool inFocus = false;
    [SerializeField] TextMeshProUGUI dramaPoster;

    private List<WeightedDrama> dramaList;
    private Action drama;
    private bool pressed = false;

    private void Start()
    {
        dramaList = new List<WeightedDrama>
        {
            new WeightedDrama(TurnOfLights, 100, "Turn off lights (bonus +20$)"),
            new WeightedDrama(TurnOfLights, 100, "Release Another Monster (bonus +50$)"),
        };

        drama = dramaList.GetRollFromWeights(new System.Random(GameSettings.Instance.GetSeed())).drama;

        dramaPoster.text = drama.ToString();
    }

    private void TurnOfLights()
    {
        Debug.Log("Turned Off Lights");
    }

    private void PressButton()
    {
        transform.position -= new Vector3(0, 0.9f, 0);
        pressed = true;
        drama.Invoke();
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
    public WeightedDrama(Action drama, int Weight, string description)
    {
        this.Weight = Weight;
        this.drama = drama;
        this.description = description;
    }

    public string description;
    public Action drama;
    [field: SerializeField] public int Weight { get; set; }

    public override string ToString()
    {
        return description;
    }
}
