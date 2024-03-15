using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletGadget : MonoBehaviour
{
    [SerializeField] float itemToggleDelay;
    
    public bool tabletEquipped;
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

    public IEnumerator SwitchGadget()
    {
        yield return new WaitForSeconds(0.1f);
        if (!holdingTabletGadget && tabletEquipped)
        {
            Toggle();
            battery.on = false;
        }
    }

    public void Toggle()
    {
        if (lastTimeUsed + itemToggleDelay <= Time.time)
        {
            lastTimeUsed = Time.time;
            tabletEquipped = !tabletEquipped;
            anim.SetTrigger("Toggle");
            //PostProcessingHandler.Instance.SetDOF(tabletEquipped);
        }
    }
}
