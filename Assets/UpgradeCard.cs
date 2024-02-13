using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCard : MonoBehaviour, IPointerClickHandler
{
    TextMeshProUGUI title;
    TextMeshProUGUI description;

    TextMeshProUGUI Title
    {
        get
        {
            if(title == null)
            {
                title = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            }
            return title;
        }
    }
    TextMeshProUGUI Description
    {
        get
        {
            if (description == null)
            {
                description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            }
            return description;
        }
    }

    Upgrade upgrade;
    public Action<Upgrade> OnChooseUpgrade { get; set; }

    public void SetUpgrade(Upgrade value)
    {
        upgrade = value;
        Title.text = value.Name;
        Description.text = value.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card clicked");
        OnChooseUpgrade(upgrade);
    }
}
