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

    public int soundIntervalMin;
    public int soundIntervalMax;
    private float timer;
    private System.Random random;
    public float localSoundMinimumDistance;
    public float localSoundMaximumDistance;
    GameObject player;

    private void Start()
    {
        UnitySingleton<PlayerManager>.Instance.OnPlayerSpawned(player =>
        {
            this.player = player;
            playerAudioSource = player.GetComponent<AudioSource>();

        });
        timer = soundIntervalMax;
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
                PlayCreepySound();
                timer = random.Next(soundIntervalMin, soundIntervalMax);
            }
        }
    }

    private void PlayCreepySound()
    {
        localNoiseMakers = localNoiseMakers?.Where(a => a != null).ToList(); //Remove destroyed noise makers
        if (random == null) random = new System.Random(GameSettings.Instance.GetSeed());
        if (localNoiseMakers == null || localNoiseMakers.Count <= 0) localNoiseMakers = GameObject.FindGameObjectsWithTag("NoiseMaker").Select(a => a.GetComponent<AudioSource>()).ToList();
        if (random.Next(1) == 0)
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
        
        AudioSource randomAudioSource = tempLocalNoiseMakers[random.Next(tempLocalNoiseMakers.Count)];
        randomAudioSource.clip = localSounds[random.Next(localSounds.Count)];
        randomAudioSource.Play();
    }

    private void PlayGlobalNoise()
    {
        if (!playerAudioSource.isPlaying)
        {
            playerAudioSource.clip = globalSounds[random.Next(globalSounds.Count)];
            playerAudioSource.Play();
        }
    }
}
