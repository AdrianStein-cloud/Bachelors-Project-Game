using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map upgrade", menuName = "Upgrades/Map upgrade")]
public class MapUpgrade : Upgrade
{
    public string layerToSeeName;

    protected override object[] Args => new object[] { };

    public override void Apply(GameObject player)
    {
        var camera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();

        camera.cullingMask |= (1 << LayerMask.NameToLayer(layerToSeeName));
    }
}
