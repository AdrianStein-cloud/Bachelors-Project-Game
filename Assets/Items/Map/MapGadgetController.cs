using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGadgetController : Item
{
    TabletGadget tablet;
    public RenderTexture tabletTexture;
    Camera camera;

    private void Awake()
    {
        tablet = GameObject.FindGameObjectWithTag("Tablet").GetComponent<TabletGadget>();
        camera = GameObject.FindGameObjectWithTag("MapCamera").GetComponentInChildren<Camera>();
    }

    public override void Select()
    {
        tablet.holdingTabletGadget = true;
        tablet.textureRenderer.SetActive(true);
        ToggleCamera(true);
    }

    public override void Deselect()
    {
        tablet.holdingTabletGadget = false;
        tablet.textureRenderer.SetActive(false);
        ToggleCamera(false);
        StartCoroutine(tablet.SwitchGadget());
    }

    void ToggleCamera(bool enable)
    {
        camera.targetTexture = enable ? tabletTexture : null;
    }

    public override void Secondary()
    {
        tablet.Toggle();
        tablet.textureRenderer.SetActive(tablet.tabletEquipped);
    }

}
