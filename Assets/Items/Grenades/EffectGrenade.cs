using System.Collections;
using Unity.VisualScripting;
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
                var coroutineRunner = collider.AddComponent<EmptyScript>();
                coroutineRunner.StartCoroutine(Effect(component, coroutineRunner));
            }
        }
    }

    IEnumerator Effect(T target, MonoBehaviour coroutineRunner)
    {
        StartEffect(target);
        yield return new WaitForSeconds(effectDuration);
        EndEffect(target);
        Destroy(coroutineRunner);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
