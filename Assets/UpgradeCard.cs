using TMPro;
using UnityEngine.UI;

public class UpgradeCard : Clickable
{
    TextMeshProUGUI title;
    TextMeshProUGUI tags;
    TextMeshProUGUI description;
    TextMeshProUGUI price;

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
    TextMeshProUGUI Tags
    {
        get
        {
            if (tags == null)
            {
                tags = transform.Find("Tags").GetComponent<TextMeshProUGUI>();
            }
            return tags;
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
    TextMeshProUGUI Price
    {
        get
        {
            if (price == null)
            {
                price = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            }
            return price;
        }
    }

    public Upgrade Upgrade { get; private set; }

    public void SetUpgrade(Upgrade value)
    {
        Upgrade = value;
        Title.text = value.Name;
        Title.color = value.Rarity.GetColor();
        Tags.text = string.Join(", ", value.Tags.GetFlags());
        Description.text = value.Description;
        Price.text = "Price: " + value.Price;
        //GetComponentInChildren<Image>().color = value.Rarity.GetColor();
    }
}
