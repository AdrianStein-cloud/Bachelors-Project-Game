using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StunGrenade : Throwable
{
    [SerializeField] float explosionRadius;
    [SerializeField] float effectDuration;
    [SerializeField] List<ParticleSystem> particles;

    static Dictionary<MonoBehaviour, Coroutine> coroutines = new();

    protected override void OnDetonate()
    {
        var particle = Instantiate(particles[Random.Range(0, particles.Count - 1)], transform.position, Quaternion.identity);
        Destroy(particle.gameObject, 2f);

        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IStunnable stun))
            {
                var component = stun.StartStun();
                if (coroutines.TryGetValue(component, out Coroutine routine))
                {
                    component.StopCoroutine(routine);
                }

                var coroutine = component.StartCoroutine(Wait(stun));
                if (!coroutines.ContainsKey(component)) coroutines.Add(component, coroutine);
            }
        }
    }

    IEnumerator Wait(IStunnable stun)
    {
        yield return new WaitForSeconds(effectDuration);
        stun.EndStun();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
