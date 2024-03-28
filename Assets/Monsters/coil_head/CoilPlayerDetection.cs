using UnityEngine;

public class CoilPlayerDetection : MonoBehaviour
{
    public float detectionRange = 500f;

    CoilInfo info;

    readonly Collider[] playerDectionHits = new Collider[5];

    private void Awake()
    {
        info = GetComponent<CoilInfo>();   
    }

    private void FixedUpdate()
    {
        int hitAmount = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, playerDectionHits, LayerMask.GetMask("Player"));
        if(hitAmount > 0)
        {
            info.Target = playerDectionHits[0].gameObject;
        }
        else
        {
            info.Target = null;
        }
    }
}
