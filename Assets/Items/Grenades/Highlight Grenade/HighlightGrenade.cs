using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightGrenade : EffectGrenade<GameObject>
{
    protected override LayerMask Mask => LayerMask.GetMask("Enemy");

    protected override void EndEffect(GameObject target)
    {
        target.SetActive(false);
        Destroy(gameObject);
    }

    protected override void StartEffect(GameObject target)
    {
        target.SetActive(true);
    }

    protected override bool TryExtractTargetComponent(Collider collider, out GameObject target)
    {
        var highlightObj = collider.GetComponentInChildren<HighlightController>(true);
        if (highlightObj != null)
        {
            target = highlightObj.gameObject;
            return true;
        }
        else
        {
            target = null;
            return false;
        }
    }
}
