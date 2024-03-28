using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CoilMovement), typeof(CoilInfo))]
public class CoilAttack : StateProcess<CoilState>
{
    public int damage;
    public float speed;

    public AudioSource footstepSource;
    public List<AudioClip> footstepSounds;
    public float footstepSoundDelay;

    private float lastStepTime;


    CoilInfo info;
    CoilMovement movement;
    NavMeshAgent agent;

    private void Awake()
    {
        info = GetComponent<CoilInfo>();
        movement = GetComponent<CoilMovement>();
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (info.Target != null & !info.IsVisible)
        {
            movement.SetTargetPosition(info.Target.transform.position, speed);
            if(!agent.isStopped) DoRandomStepSound();
        }
        if(info.Target == null)
        {
            StateController.SwitchState(CoilState.Roam);
        }
    }

    void DoRandomStepSound()
    {
        if (lastStepTime + footstepSoundDelay > Time.time) return;

        lastStepTime = Time.time;
        footstepSource.clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
        footstepSource.PlayOneShot(footstepSource.clip);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<PlayerHealth>();
            health.TakeDamage(damage);
        }
    }
}
