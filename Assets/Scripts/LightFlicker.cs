using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    GameObject[] childObjects;

    [SerializeField] float offFlickerSpeedMin = 0.05f;
    [SerializeField] float offFlickerSpeedMax = 0.4f;
    [SerializeField] float onFlickerSpeedMin = 0.05f;
    [SerializeField] float onFlickerSpeedMax = 2f;

    private void Start()
    {
        childObjects = transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        Random.InitState(GameSettings.Instance.GetSeed());
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            foreach (GameObject light in childObjects)
            {
                light.SetActive(false);
                
            }
            yield return new WaitForSeconds(Random.Range(offFlickerSpeedMin, offFlickerSpeedMax));
            foreach (GameObject light in childObjects)
            {
                light.SetActive(true);

            }
            yield return new WaitForSeconds(Random.Range(onFlickerSpeedMin, onFlickerSpeedMax));
        }
    }
}
