using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGadgetController : Item
{
    TabletGadget tablet;
    public RenderTexture tabletTexture;
    Camera mapCam;

    public float batteryDrain;

    bool gadgetOn;

    private void Start()
    {
        tablet = GameObject.FindGameObjectWithTag("Tablet").GetComponent<TabletGadget>();
        mapCam = GameObject.FindGameObjectWithTag("MapCamera").GetComponentInChildren<Camera>();

        tablet.battery.OnDead += () => { if (gadgetOn) gadgetOn = false; };
        
    }

    public override void Select()
    {
        tablet.holdingTabletGadget = true;
        tablet.textureRenderer.SetActive(false);
        tablet.battery.Select();
        if (!tablet.equipped) tablet.Toggle();
        else ToggleGadget();
    }

    public override void Deselect()
    {
        tablet.holdingTabletGadget = false;

        tablet.battery.Deselect();
        if (gadgetOn) ToggleGadget();
        tablet.SwitchGadget();
    }

    void ToggleGadget()
    {
        if (tablet.battery.Dead) return;
        gadgetOn = !gadgetOn;

        tablet.battery.ToggleBattery(batteryDrain);
        ToggleCamera(gadgetOn);
        tablet.textureRenderer.SetActive(gadgetOn);
    }

    void ToggleCamera(bool enable)
    {
        mapCam.targetTexture = enable ? tabletTexture : null;
    }

    public override void Primary() => Secondary();

    public override void Secondary()
    {
        ToggleGadget();
    }
}
