using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentTrigger : MonoBehaviour
{
    [SerializeField] int triggerChance;
    private System.Random random;
    private Rigidbody rb;
    private AudioSource source;

    private void Start()
    {
        random = new System.Random(GameSettings.Instance.GetSeed());
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.Play();
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (random.Next(0, 100) < triggerChance)
            {
                rb.isKinematic = false;
            }
        }
    }
}
