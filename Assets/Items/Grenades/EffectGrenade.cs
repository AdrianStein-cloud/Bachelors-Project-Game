using System.Collections;
using UnityEngine;

public abstract class EffectGrenade<T> : Throwable
{
    [SerializeField] float explosionRadius;
    [SerializeField] float effectDuration;

    protected abstract LayerMask Mask { get; }
    protected abstract bool TryExtractTargetComponent(Collider collider, out T target);
    protected abstract void StartEffect(T target);
    protected abstract void EndEffect(T target);

    protected override void OnDetonate()
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, Mask);
        foreach (var collider in colliders)
        {
            if (TryExtractTargetComponent(collider, out T component))
            {
                StartCoroutine(Effect(component));
            }
        }
    }

    IEnumerator Effect(T target)
    {
        if (target == null) Debug.LogWarning("Grenade target was null");
        StartEffect(target);
        yield return new WaitForSeconds(effectDuration);
        if (target == null) Debug.LogWarning("Grenade target was null");
        EndEffect(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
