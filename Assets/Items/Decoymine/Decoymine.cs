using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoymine : MonoBehaviour
{
    [SerializeField] float soundRadius;
    [SerializeField] float soundDuration;
    [SerializeField] float blinkDelay;

    public bool Activated { get; private set; }

    AudioSource source;
    GameObject outline;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Init()
    {
        GetComponent<Collider>().enabled = true;
        outline = transform.GetChild(0).gameObject;
        outline.SetActive(true);
    }

    public void Activate()
    {
        if (Activated) return;
        Activated = true;
        SetOutline(false);

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

    public void SetOutline(bool value) => outline.SetActive(!Activated && value);
}
