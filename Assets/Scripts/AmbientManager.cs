using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> localSounds;
    [SerializeField] private List<AudioClip> globalSounds;
    private List<AudioSource> localNoiseMakers;
    private AudioSource playerAudioSource;

    public float soundInterval;
    private float timer;
    private System.Random random;
    public float localSoundMinimumDistance;
    public float localSoundMaximumDistance;
    GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerAudioSource = player.GetComponent<AudioSource>();
        timer = soundInterval;
    }

    private void Update()
    {
        if (GameSettings.Instance.PlayerInDungeon)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = soundInterval;
                PlayCreepySound();
            }
        }
    }

    private void PlayCreepySound()
    {
        if (random == null) random = new System.Random(GameSettings.Instance.GetSeed());
        if (localNoiseMakers == null) localNoiseMakers = GameObject.FindGameObjectsWithTag("NoiseMaker").Select(a => a.GetComponent<AudioSource>()).ToList();
        if (random.Next(0) == 0)
        {
            PlayLocalNoise();
        }
        else
        {
            PlayGlobalNoise();
        }
    }

    private void PlayLocalNoise()
    {
        var tempLocalNoiseMakers = localNoiseMakers.Where(a => Vector3.Distance(player.transform.position, a.gameObject.transform.position) >= localSoundMinimumDistance && Vector3.Distance(player.transform.position, a.gameObject.transform.position) <= localSoundMaximumDistance).ToList();
        if (tempLocalNoiseMakers.Count <= 0) return;
        
        AudioSource randomAudioSource = tempLocalNoiseMakers[random.Next(tempLocalNoiseMakers.Count - 1)];
        randomAudioSource.clip = localSounds[random.Next(localSounds.Count - 1)];
        randomAudioSource.Play();
    }

    private void PlayGlobalNoise()
    {
        playerAudioSource.clip = globalSounds[random.Next(globalSounds.Count - 1)];
        playerAudioSource.Play();
    }
}
