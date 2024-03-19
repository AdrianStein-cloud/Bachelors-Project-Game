using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentTrigger : MonoBehaviour
{
    [SerializeField] int triggerChance;
    private System.Random random;
    private Rigidbody rb;

    private void Start()
    {
        random = new System.Random(GameSettings.Instance.GetSeed());
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (random.Next(0, 100) < triggerChance)
            {
                rb.isKinematic = false;
                Destroy(this);
            }
        }
    }
}
