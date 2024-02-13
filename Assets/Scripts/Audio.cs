using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    Dictionary<string, AudioSource> sources;

    private void Awake()
    {
        sources = new();

        foreach (Transform audio in transform.Find("Audio"))
        {
            var source = audio.GetComponent<AudioSource>();
            sources.Add(audio.gameObject.name, source);
        }
    }

    public void Play(string name)
    {
        sources[name].Play();
    }

    public void Stop(string name)
    {
        sources[name].Stop();
    }

    public void Pause(string name)
    {
        sources[name].Pause();
    }

    public bool IsPlaying(string name) => sources[name].isPlaying;
}
