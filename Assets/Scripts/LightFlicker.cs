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
    [SerializeField] int failPercentage = 20;

    private void Start()
    {
        glowMaterial = bulb.GetComponent<Renderer>().material;

        childObjects = transform.Cast<Transform>().Select(t => t.gameObject).ToArray();

        lightIntensities = new float[childObjects.Length];

        for (int i = 0; i < childObjects.Length; i++)
        {
            lightIntensities[i] = childObjects[i].GetComponent<Light>().intensity;
        }

        bool failed = Random.Range(0, 100) < failPercentage;

        if (!failed)
        {
            StartCoroutine(Flicker());
        }
        else
        {
            TurnOff();
        }
    }

    private void TurnOff()
    {
        Debug.Log("Turned of light");
        for (int i = 0; i < childObjects.Length; i++)
        {
            childObjects[i].GetComponent<Light>().intensity = 0;
            glowMaterial.DisableKeyword("_EMISSION");
        }
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
