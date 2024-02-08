using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> Upgrades;

    public GameObject card;
    public GameObject player;

    Upgrade upgrade;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) DisplayRandomUpgrade();
        if (Input.GetKeyDown(KeyCode.P)) upgrade.Apply(player);
    }

    public void DisplayRandomUpgrade()
    {
        upgrade = Upgrades[Random.Range(0, Upgrades.Count)];
        card.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = upgrade.Name;
        card.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = upgrade.Description;
    }
}
