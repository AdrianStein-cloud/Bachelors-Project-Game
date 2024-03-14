using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletGadget : MonoBehaviour
{
    [SerializeField] float itemToggleDelay;
    
    public bool tabletEquipped;
    public GameObject textureRenderer;

    Animator anim;
    float lastTimeUsed;

    public bool holdingTabletGadget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator SwitchGadget()
    {
        yield return new WaitForSeconds(0.1f);
        if (!holdingTabletGadget && tabletEquipped)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (lastTimeUsed + itemToggleDelay <= Time.time)
        {
            lastTimeUsed = Time.time;
            tabletEquipped = !tabletEquipped;
            anim.SetTrigger("Toggle");
            PostProcessingHandler.Instance.SetDOF(tabletEquipped);
        }
    }
}
