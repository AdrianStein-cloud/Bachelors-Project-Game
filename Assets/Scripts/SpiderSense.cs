using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpiderSense : MonoBehaviour
{

    public int senseRange;
    public float cooldown;


    public AudioClip sound;
    public LayerMask enemyLayer;
    public int verticalSenseRange;

    bool onCooldown = false;

    readonly Collider[] colliders = new Collider[50];

    private Image spiderSenseEffect;

    private void Start()
    {
        spiderSenseEffect = GameSettings.Instance.canvas.transform.Find("SpiderSense").GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (!onCooldown)
        {
            Physics.OverlapSphereNonAlloc(transform.position, senseRange, colliders, enemyLayer);
            float minDistance = senseRange;

            foreach(Collider c in colliders)
            {
                if (c!= null && IsOnSameFloor(c))
                {
                    float newDistance = Vector3.Distance(transform.position, c.transform.position);

                    if(newDistance < minDistance)
                    {
                        minDistance = newDistance;
                    }
                }
            }

            onCooldown = true;
            StartCoroutine(Cooldown());
            TingleSpiderSense(minDistance);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Cooldown());
    }

    bool IsOnSameFloor(Collider col)
    {
        float verticalDiff = Mathf.Abs(transform.position.y - col.transform.position.y);

        return verticalDiff <= verticalSenseRange;
    }

    void TingleSpiderSense(float distance)
    {
        Color color = new Color(1, 1, 1, 1 - distance/senseRange);
        spiderSenseEffect.color = color;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
        yield break;
    }

}
