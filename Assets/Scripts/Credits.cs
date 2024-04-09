using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private List<Credit> assetsList;
    [SerializeField] private TextMeshProUGUI creditsText;

    private void Start()
    {
        StringBuilder stringBuilder = new StringBuilder("\n<b>Credits</b>\n\n");

        stringBuilder.Append("<b>Assets</b>\n");

        foreach (var asset in assetsList)
        {
            stringBuilder.Append(asset.contribution + " - " + asset.contributorName + "\n");
            stringBuilder.Append(asset.contributorUrl + "\n\n");
        }

        creditsText.text = stringBuilder.ToString();
    }
}

[System.Serializable]
public class Credit
{
    public string contribution;
    public string contributorName;
    public string contributorUrl;
}
