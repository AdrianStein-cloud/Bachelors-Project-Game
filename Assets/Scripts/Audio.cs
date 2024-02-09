using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] List<NamedAudio> audioClips;

    Dictionary<string, AudioSource> sources;

    private void Awake()
    {
        sources = new();
        foreach (var audio in audioClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = audio.Clip;
            source.playOnAwake = false;
            sources.Add(audio.Name, source);
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

[Serializable]
public class NamedAudio
{
    public string Name;
    public AudioClip Clip;
}
