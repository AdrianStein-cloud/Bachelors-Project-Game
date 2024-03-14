using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;

public class WandererSounds : MonoBehaviour
{
    [Header("Misc")]
    public AudioClip ambianceSound;

    [Header("Scream")]
    public AudioSource monsterSource;
    public List<AudioClip> screamSounds;

    [Header("Footsteps")]
    public AudioSource footstepSource;
    public List<AudioClip> footstepSounds;
    public List<AudioClip> runningFootstepSounds;

    private List<AudioClip> tempFootsteps = new List<AudioClip>();
    private List<AudioClip> tempRunningFootsteps = new List<AudioClip>();

    public Action OnScreamEnd;

    public void PlayFootstepSound()
    {
        if (footstepSounds.Count == 0) return;
        if (tempFootsteps.Count <= 0) tempFootsteps = new List<AudioClip>(footstepSounds);

        var clip = tempFootsteps[Random.Range(0, tempFootsteps.Count)];
        footstepSource.clip = clip;
        footstepSource.pitch = 0.6f;
        footstepSource.PlayOneShot(clip);
        tempFootsteps.Remove(clip);
    }

    public void PlayRunningFootstepSound()
    {
        if (runningFootstepSounds.Count == 0) return;
        if (tempRunningFootsteps.Count <= 0) tempRunningFootsteps = new List<AudioClip>(runningFootstepSounds);

        var clip = tempRunningFootsteps[Random.Range(0, tempRunningFootsteps.Count)];
        footstepSource.clip = clip;
        footstepSource.pitch = 1;
        footstepSource.PlayOneShot(clip);
        tempRunningFootsteps.Remove(clip);
    }

    public void PlayScreechSound()
    {
        if (screamSounds.Count == 0) return;
        var clip = screamSounds[Random.Range(0, screamSounds.Count)];
        monsterSource.clip = clip;
        monsterSource.Play();
    }

    public void ScreechEnded()
    {
        OnScreamEnd?.Invoke();
    }

    public void PlayAmbiance()
    {
        monsterSource.clip = ambianceSound;
        monsterSource.Play();
    }
}
