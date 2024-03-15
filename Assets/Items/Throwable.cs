using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] float detonationTime;

    bool canDetonate = true;

    private void Awake()
    {
        if (detonationTime > 0)
        {
            StartCoroutine(DetonateAfterTime());
        }
    }

    private IEnumerator DetonateAfterTime()
    {
        yield return new WaitForSeconds(detonationTime);
        Detonate();
        //Destroy(gameObject);
    }

    protected abstract void Detonate();

    private void OnCollisionEnter(Collision other)
    {
        if (canDetonate && detonationTime <= 0 && !other.gameObject.CompareTag("Player"))
        {
            canDetonate = false;
            Detonate();
            //Destroy(gameObject);
        }
    }
}
