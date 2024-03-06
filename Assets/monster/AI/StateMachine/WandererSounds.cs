using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererSounds : MonoBehaviour
{

    public AudioSource footstepSource;
    public List<AudioClip> footstepSounds;
    public List<AudioClip> runningFootstepSounds;

    private List<AudioClip> tempFootsteps = new List<AudioClip>();
    private List<AudioClip> tempRunningFootsteps = new List<AudioClip>();

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
}
