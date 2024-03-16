using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGadgetController : Item
{
    TabletGadget tablet;
    public RenderTexture tabletTexture;
    Camera camera;

    public float batteryDrain;

    private void Awake()
    {
        tablet = GameObject.FindGameObjectWithTag("Tablet").GetComponent<TabletGadget>();
        camera = GameObject.FindGameObjectWithTag("MapCamera").GetComponentInChildren<Camera>();
    }

    public override void Select()
    {
        tablet.holdingTabletGadget = true;
        if (!tablet.battery.Dead) tablet.textureRenderer.SetActive(true);
        ToggleCamera(true);
        tablet.battery.Select();
        tablet.battery.batteryDrain = this.batteryDrain;
        if (!tablet.battery.on && tablet.tabletEquipped) tablet.battery.ToggleBattery();

        if (!tablet.tabletEquipped) tablet.Toggle();
    }

    public override void Deselect()
    {
        tablet.holdingTabletGadget = false;
        tablet.textureRenderer.SetActive(false);
        ToggleCamera(false);
        tablet.battery.Deselect();
        tablet.SwitchGadget();
    }

    void ToggleCamera(bool enable)
    {
        camera.targetTexture = enable ? tabletTexture : null;
    }

    public override void Primary() => Secondary();

    public override void Secondary()
    {
        if (tablet.Toggle()) tablet.battery.ToggleBattery();
        if (!tablet.battery.Dead) tablet.textureRenderer.SetActive(tablet.tabletEquipped);
    }
}
