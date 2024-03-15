using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGrenade : Throwable
{
    [SerializeField] float explosionRadius;
    [SerializeField] float speedMultiplier;
    [SerializeField] float sensitivityMultiplier;
    [SerializeField] float effectTime;
    [SerializeField] float exposureAmount;
    [SerializeField] List<ParticleSystem> particles;

    PostProcessingHandler pp;

    static Coroutine effectRoutine;

    protected override void Start()
    {
        pp = PostProcessingHandler.Instance;
    }

    protected override void OnDetonate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            var particle = Instantiate(particles[Random.Range(0, particles.Count - 1)], transform.position, Quaternion.identity);
            Destroy(particle.gameObject, 2f);
            if (collider.TryGetComponent(out PlayerMovement player))
            {
                CameraShake.Instance.Shake(CameraShake.Instance.Defaultpreset);

                if (effectRoutine != null) player.StopCoroutine(effectRoutine);
                effectRoutine = player.StartCoroutine(Wait(player));
            }
        }
    }

    IEnumerator Wait(PlayerMovement player)
    {
        var cam = Camera.main.GetComponentInParent<CameraController>();

        pp.SetChromaticAberration(0.1f, 1f);
        pp.SetPostExposure(0.1f, exposureAmount);
        player.SpeedMultiplier = speedMultiplier;
        cam.SensitivityMultiplier = sensitivityMultiplier;

        yield return new WaitForSeconds(effectTime);
        pp.SetChromaticAberration(1f);
        pp.ResetPostExposure(1f);
        player.SpeedMultiplier = 1f;
        cam.SensitivityMultiplier = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
