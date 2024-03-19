using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTrigger : MonoBehaviour
{
    private System.Random _random;
    [SerializeField] private ParticleSystem particleSystem;
    public int triggerChance;
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _random = new System.Random(GameSettings.Instance.GetSeed());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_random.Next(0, 100) < triggerChance)
            {
                if(_particleSystem == null)
                    _particleSystem = Instantiate(particleSystem, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                _particleSystem.Play();
            }
        }
    }
}