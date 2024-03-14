using System.Collections;
using UnityEngine;

public class SpiderSense : MonoBehaviour
{

    public int senseRange;
    public float cooldown;


    public AudioClip sound;
    public LayerMask enemyLayer;
    public int verticalSenseRange;

    bool onCooldown = false;

    readonly Collider[] colliders = new Collider[50];
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!onCooldown)
        {
           Physics.OverlapSphereNonAlloc(transform.position, senseRange, colliders, enemyLayer);
            foreach(Collider c in colliders)
            {
                if (c!= null && IsOnSameFloor(c))
                {
                    onCooldown = true;
                    StartCoroutine(Cooldown());
                    TingleSpiderSense();
                }
            }
        }
    }

    bool IsOnSameFloor(Collider col)
    {
        float verticalDiff = Mathf.Abs(transform.position.y - col.transform.position.y);

        return verticalDiff <= verticalSenseRange;
    }

    void TingleSpiderSense()
    {
        source.PlayOneShot(sound);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

}
