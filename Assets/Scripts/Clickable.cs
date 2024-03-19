using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    public Action OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        OnClick();
    }
}
