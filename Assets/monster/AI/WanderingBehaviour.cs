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

    private AudioSource monsterSource, footstepSource;
    public GameObject footstepSourceGO;

    public List<AudioClip> footstepSounds;
    public List<AudioClip> screamSounds;
    private List<AudioClip> tempFootsteps = new List<AudioClip>();

    public float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        monsterSource = GetComponent<AudioSource>();
        footstepSource = footstepSourceGO.GetComponent<AudioSource>();
    }

    public void UpdateState(WanderingState newstate)
    {
        if (state != newstate)
        {
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
        footstepSource.Play();
        tempFootsteps.Remove(clip);
    }

    public void PlayScreechSound()
    {
        var clip = screamSounds[Random.Range(0, screamSounds.Count)];
        monsterSource.clip = clip;
        monsterSource.Play();
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
