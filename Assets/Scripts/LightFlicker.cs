using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    GameObject[] childObjects;
    float[] lightIntensities;

    [SerializeField] GameObject bulb;

    private Material glowMaterial;

    [SerializeField] float offFlickerSpeedMin = 0.05f;
    [SerializeField] float offFlickerSpeedMax = 0.4f;
    [SerializeField] float onFlickerSpeedMin = 0.05f;
    [SerializeField] float onFlickerSpeedMax = 2f;

    private void Start()
    {
        glowMaterial = bulb.GetComponent<Renderer>().material;

        childObjects = transform.Cast<Transform>().Select(t => t.gameObject).ToArray();

        lightIntensities = new float[childObjects.Length];

        for (int i = 0; i < childObjects.Length; i++)
        {
            lightIntensities[i] = childObjects[i].GetComponent<Light>().intensity;
        }

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(2);
        while (true)
        {
            for(int i = 0; i < childObjects.Length; i++)
            {
                FlickerOff(childObjects[i].GetComponent<Light>(), lightIntensities[i]);
                glowMaterial.DisableKeyword("_EMISSION");
            }
            yield return new WaitForSeconds(Random.Range(offFlickerSpeedMin, offFlickerSpeedMax));
            for (int i = 0; i < childObjects.Length; i++)
            {
                FlickerOn(childObjects[i].GetComponent<Light>(), lightIntensities[i]);
                glowMaterial.EnableKeyword("_EMISSION");
            }
            yield return new WaitForSeconds(Random.Range(onFlickerSpeedMin, onFlickerSpeedMax));
        }
    }

    private void FlickerOn(Light light, float maxIntensity)
    {
        light.intensity = maxIntensity;
    }

    private void FlickerOff(Light light, float maxIntensity)
    {
        light.intensity = Random.Range(0, maxIntensity/3);
    }
}
