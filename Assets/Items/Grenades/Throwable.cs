using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] AudioSource throwSource;
    [SerializeField] AudioSource detonateSource;
    [SerializeField] float detonationTime;

    bool canDetonate = true;

    [SerializeField] private bool hasExplosionForce;
    [SerializeField] private float explosionForce, explosionForceRadius;

    private void Awake()
    {
        detonationTime = Stats.Instance.grenade.detonateOnImpact ? 0 : detonationTime;

        throwSource.Play();

        if (detonationTime > 0)
        {
            StartCoroutine(DetonateAfterTime());
        }
    }

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
        if (hasExplosionForce) DoExplosionForce();
    }

    protected abstract void OnDetonate();

    private void OnCollisionEnter(Collision other)
    {
        if (canDetonate && detonationTime <= 0 && !other.gameObject.CompareTag("Player"))
        {
            Detonate();
        }
    }

    void DoExplosionForce()
    {
        var hits = Physics.OverlapSphere(transform.position, explosionForceRadius);

        foreach(var hit in hits)
        {
            if (hit.TryGetComponent(out Rigidbody rb)) rb.AddExplosionForce(explosionForce, transform.position, explosionForceRadius);
        }

    }
}
