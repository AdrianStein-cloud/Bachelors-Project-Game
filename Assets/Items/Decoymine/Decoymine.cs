using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoymine : MonoBehaviour
{
    [SerializeField] float soundRadius;
    [SerializeField] float soundDuration;
    [SerializeField] float blinkDelay;

    AudioSource source;
    bool activated;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Init()
    {
        GetComponent<Collider>().enabled = true;
    }

    public void Activate()
    {
        if (activated) return;
        activated = true;

        StartCoroutine(Blink());

        var colliders = Physics.OverlapSphere(transform.position, soundRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDistractable entity))
            {
                entity.Distract(transform.position);
            }
        }
    }

    IEnumerator Blink()
    {
        var material = GetComponent<MeshRenderer>().material;
        material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(blinkDelay);

        var time = Time.time;
        while (Time.time < time + soundDuration)
        {
            source.Play();
            material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(blinkDelay);
            material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(blinkDelay);
        }
    }
}
