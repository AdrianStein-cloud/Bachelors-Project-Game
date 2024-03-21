using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentTrigger : MonoBehaviour
{
    [SerializeField] int triggerChance;
    private static System.Random random;
    private Rigidbody rb;
    private AudioSource source;

    private void Start()
    {
        if (random == null) random = new System.Random(GameSettings.Instance.GetSeed());
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (rb.isKinematic) return;
        rb.AddForce(Physics.gravity * rb.mass, ForceMode.Acceleration);
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
