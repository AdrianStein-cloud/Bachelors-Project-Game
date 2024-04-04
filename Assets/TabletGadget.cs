using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletGadget : MonoBehaviour
{
    [SerializeField] float itemToggleDelay;

    public bool equipped;
    public GameObject textureRenderer;

    public BatteryItem batteryPrefab;
    public BatteryItem battery;

    Animator anim;
    float lastTimeUsed;

    public bool holdingTabletGadget;

    // Start is called before the first frame update
    void Start()
    {
        battery = Instantiate(batteryPrefab, this.transform);
        battery.OnDead += () => textureRenderer.SetActive(false);

        anim = GetComponent<Animator>();
    }

    public void SwitchGadget()
    {
        StopAllCoroutines();
        StartCoroutine(switchGadget());
    }
    IEnumerator switchGadget()
    {
        yield return new WaitForSeconds(0.02f);
        if (!holdingTabletGadget && equipped)
        {
            Toggle();
        }
    }

    /// <summary>
    /// Describes whether or not the tablet actually toggled. Is false if on cooldown
    /// </summary>
    /// <returns></returns>
    public bool Toggle()
    {
        lastTimeUsed = Time.time;
        equipped = !equipped;
        anim.SetTrigger("Toggle");
        //PostProcessingHandler.Instance.SetDOF(tabletEquipped);
        return true;
    }
}
