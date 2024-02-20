using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] float cooldown;

    AudioSource source;
    bool placed;
    bool beeping;
    bool canBeep;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        canBeep = true;
    }

    public void Init()
    {
        laser.SetActive(true);
        placed = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            source.Play();
        }

        if (!placed) return;

        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, Mathf.Infinity, hitLayer))
        {
            var distance = Vector3.Distance(transform.position, hit.point);
            var laserScale = 1 / transform.localScale.x / 2 * distance;
            var position = laser.transform.localPosition;
            position.y = laserScale;
            laser.transform.localPosition = position;

            var scale = laser.transform.localScale;
            scale.y = laserScale;
            laser.transform.localScale = scale;

            if (Physics.Raycast(transform.position, transform.up, distance, detectionLayer))
            {
                if (!beeping && canBeep)
                {
                    source.Play();
                    canBeep = false;
                    StartCoroutine(Wait());
                }
                beeping = true;
            }
            else beeping = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(cooldown);
        canBeep = true;
    }
}
