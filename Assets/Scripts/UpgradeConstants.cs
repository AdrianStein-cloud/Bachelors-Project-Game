using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeConstants : MonoBehaviour
{
    public List<SerializableTuple<Rarity, int>> price;
    public List<SerializableTuple<Rarity, int>> chance;
    public List<SerializableTuple<Rarity, Color>> colors;

    public static UpgradeConstants Instance;

    private void Awake()
    {
        Instance = this;
    }
}
