using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI waveCounter;

    private void Start()
    {
        OnWaveChanged(0);
        gameManager = FindObjectOfType<GameManager>();
        gameManager.OnDungeonGenerated += OnWaveChanged;
    }

    private void OnWaveChanged(int obj)
    {
        waveCounter.text = obj.ToString();
    }
}
