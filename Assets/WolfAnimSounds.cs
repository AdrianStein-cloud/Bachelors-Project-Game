using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimSounds : MonoBehaviour
{
    WolfSounds sounds;

    private void Awake()
    {
        sounds = GetComponentInParent<WolfSounds>();
    }

    public void PlayBite()
    {
        sounds.BiteSound();
    }

    public void PlayCry()
    {
        sounds.DeadSound();
    }

    public void PlayFootsteps()
    {
        sounds.FootstepSound();
    }
}
