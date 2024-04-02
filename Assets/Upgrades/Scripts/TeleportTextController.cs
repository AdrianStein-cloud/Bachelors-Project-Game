using TMPro;
using UnityEngine;

public class TeleportTextController : MonoBehaviour
{
    [SerializeField] Color canTpColor = Color.green;
    [SerializeField] Color cannotTpColor = Color.red;
    [SerializeField] string canTpText;
    [SerializeField] string cannotTpText;
    [SerializeField] string chargingText;

    TextMeshProUGUI textMesh;

    private void Awake()
    {
        UnitySingleton<TeleportTextController>.BecomeSingleton(this);
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Display(bool validDestination, bool ready)
    {
        textMesh.color = validDestination ? canTpColor : cannotTpColor;

        if (ready & validDestination)
        {
            textMesh.text = canTpText;
        }
        else if(ready & !validDestination)
        {
            textMesh.text = cannotTpText;
        }
        else
        {
            textMesh.text = chargingText;
        }
    }

    public void Hide()
    {
        textMesh.text = "";
    }
}
