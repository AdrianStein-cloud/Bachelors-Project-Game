using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGrenade : Throwable
{
    [SerializeField] float explosionRadius;
    [SerializeField] List<ParticleSystem> particles;

    protected override void Detonate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            var particle = Instantiate(particles[Random.Range(0, particles.Count - 1)], transform.position, Quaternion.identity);
            Destroy(particle.gameObject, 2f);
            if (collider.CompareTag("Player"))
            {
                CameraShake.Instance.Shake(CameraShake.Instance.Defaultpreset);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
