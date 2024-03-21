using System.Collections;
using UnityEngine;

public abstract class EffectGrenade<T> : Throwable
{
    [SerializeField] float explosionRadius;
    [SerializeField] float effectDuration;

    protected abstract bool IsColliderTarget(Collider collider);
    protected abstract T ExtractTargetComponent(Collider collider);
    protected abstract void StartEffect(T target);
    protected abstract void EndEffect(T target);

    protected override void OnDetonate()
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (IsColliderTarget(collider))
            {
                T component = ExtractTargetComponent(collider);

                StartCoroutine(Effect(component));
            }
        }
    }

    IEnumerator Effect(T target)
    {
        if (target == null) Debug.LogError("Grenade target was null");
        StartEffect(target);
        yield return new WaitForSeconds(effectDuration);
        if (target == null) Debug.LogError("Grenade target was null");
        EndEffect(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
