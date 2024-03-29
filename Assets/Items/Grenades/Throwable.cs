using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] AudioSource throwSource;
    [SerializeField] AudioSource detonateSource;
    [SerializeField] float detonationTime;

    bool canDetonate = true;

    private void Awake()
    {
        throwSource.Play();

        if (detonationTime > 0)
        {
            StartCoroutine(DetonateAfterTime());
        }
    }

    protected virtual void Start() { }

    private IEnumerator DetonateAfterTime()
    {
        yield return new WaitForSeconds(detonationTime);
        Detonate();
    }

    void Detonate()
    {
        canDetonate = false;
        detonateSource.Play();
        OnDetonate();
        //Destroy(gameObject);
    }

    protected abstract void OnDetonate();

    private void OnCollisionEnter(Collision other)
    {
        if (canDetonate && detonationTime <= 0 && !other.gameObject.CompareTag("Player"))
        {
            Detonate();
        }
    }
}
