using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : MonoBehaviour
{
    Animator anim;
    public WanderingState state;

    public List<GameObject> roomsCopy;
    public GameObject wanderingTo;
    public bool once = false;
    public GameObject doorToOpen;

    public GameObject currentRoom;

    public bool hasReachedRoom = true;

    public Vector3 lastLocation;

    private AudioSource monsterSource, footstepSource, attackSource;
    public GameObject footstepSourceGO, attackSourceGO;

    public List<AudioClip> screamSounds;

    public List<AudioClip> footstepSounds;
    public List<AudioClip> runningFootstepSounds;
    private List<AudioClip> tempFootsteps = new List<AudioClip>();
    private List<AudioClip> tempRunningFootsteps = new List<AudioClip>();

    public float lastAttackTime;
    public float attackDelay;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        monsterSource = GetComponent<AudioSource>();
        footstepSource = footstepSourceGO.GetComponent<AudioSource>();
        attackSource = attackSourceGO.GetComponent<AudioSource>();
    }

    public void UpdateState(WanderingState newstate)
    {
        if (state != newstate)
        {
            if (newstate == WanderingState.Attack) 
            {
                anim.SetTrigger("Attack");
                return;
            }

            anim.SetBool(state.ToString(), false);
            state = newstate;
            anim.SetBool(state.ToString(), true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.gameObject;
        }
    }

    public void PlayFootstepSound()
    {
        if (tempFootsteps.Count <= 0) tempFootsteps = new List<AudioClip>(footstepSounds);

        var clip = tempFootsteps[Random.Range(0, tempFootsteps.Count)];
        footstepSource.clip = clip;
        footstepSource.pitch = 0.6f;
        footstepSource.PlayOneShot(clip);
        tempFootsteps.Remove(clip);
    }

    public void PlayRunningFootstepSound()
    {
        if (tempRunningFootsteps.Count <= 0) tempRunningFootsteps = new List<AudioClip>(runningFootstepSounds);

        var clip = tempRunningFootsteps[Random.Range(0, tempRunningFootsteps.Count)];
        footstepSource.clip = clip;
        footstepSource.pitch = 1;
        footstepSource.PlayOneShot(clip);
        tempRunningFootsteps.Remove(clip);
    }

    public void PlayScreechSound()
    {
        var clip = screamSounds[Random.Range(0, screamSounds.Count)];
        monsterSource.clip = clip;
        monsterSource.Play();
    }

    public void PlayAttackSound()
    {
        attackSource.Play();
    }
}

public enum WanderingState
{
    Wander,
    Search,
    Chase,
    Attack,
    Dead,
    Scream
}