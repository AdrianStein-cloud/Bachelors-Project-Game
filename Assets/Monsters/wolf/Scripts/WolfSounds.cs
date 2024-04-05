using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSounds : MonoBehaviour
{
    public AudioSource stepSource, attackSource, voiceSource, biteSource;

    public List<AudioClip> footstepSounds;
    List<AudioClip> tempFootsteps;

    public AudioClip attackSound, runawaySound, dieSound, biteSound;

    private void Awake()
    {
        tempFootsteps = new List<AudioClip>();
    }

    public void FootstepSound()
    {
        if (footstepSounds.Count == 0) return;
        if (tempFootsteps.Count <= 0) tempFootsteps = new List<AudioClip>(footstepSounds);

        var clip = tempFootsteps[Random.Range(0, tempFootsteps.Count)];
        stepSource.clip = clip;
        stepSource.pitch = 0.6f;
        stepSource.PlayOneShot(clip);
        tempFootsteps.Remove(clip);
    }

    public void AttackSound()
    {
        voiceSource.clip = attackSound;
        voiceSource.Play();
    }

    public void BiteSound()
    {
        biteSource.clip = biteSound;
        biteSource.Play();
    }

    public void RunawaySound()
    {
        voiceSource.clip = runawaySound;
        voiceSource.Play();
    }

    public void DeadSound()
    {
        voiceSource.clip = dieSound;
        voiceSource.Play();
    }

}
