using TMPro;
using UnityEngine;

public class TeleportTextController : MonoBehaviour
{
    [SerializeField] Color canTpColor = Color.green;
    [SerializeField] Color cannotTpColor = Color.red;
    [SerializeField] string canTpText;
    [SerializeField] string cannotTpText;

    TextMeshProUGUI textMesh;

    private void Awake()
    {
        UnitySingleton<TeleportTextController>.BecomeSingleton(this);
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Display(bool canTeleport)
    {
        if (canTeleport)
        {
            textMesh.text = canTpText;
            textMesh.color = canTpColor;
        }
        else
        {
            textMesh.text = cannotTpText;
            textMesh.color = cannotTpColor;
        }
    }

    public void Hide()
    {
        textMesh.text = "";
    }
}
