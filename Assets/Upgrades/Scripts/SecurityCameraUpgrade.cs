using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraUpgrade : ItemUpgrade
{
    public GameObject tabletPrefab;

    public override void Apply(GameObject player)
    {
        var item = ApplyItem(player);
    }
}
