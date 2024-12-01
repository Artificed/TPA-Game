using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioClip campfireClip;

    [SerializeField] private List<AudioClip> playerFootStepClips;
    
    [SerializeField] private List<AudioClip> enemyAlertClips;
    [SerializeField] private List<AudioClip> punchClips;
    [SerializeField] private List<AudioClip> swordClips;
    [SerializeField] private List<AudioClip> criticalHitClips;

    [SerializeField] private List<AudioClip> takeDamageClips;
    [SerializeField] private List<AudioClip> deathClips;

    [SerializeField] private AudioClip lifeStealClip;
    [SerializeField] private AudioClip bashClip;
    
    [SerializeField] private AudioSource soundFXObject;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource, clipLength);
    }
    
    private void PlayRandomClip(List<AudioClip> clips, Transform spawnTransform, float volume)
    {
        if (clips == null || clips.Count == 0) return;

        AudioClip randomClip = clips[Random.Range(0, clips.Count)];
        PlaySoundFXClip(randomClip, spawnTransform, volume);
    }
    
    public void PlayCampfireClip(Transform spawnTransform, float volume = 0.5f)
    {
        if (campfireClip != null)
        {
            PlaySoundFXClip(campfireClip, spawnTransform, volume);
        }
    }

    public void PlayPlayerFootStepClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(playerFootStepClips, spawnTransform, volume);
    }

    public void PlayEnemyAlertClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(enemyAlertClips, spawnTransform, volume);
    }

    public void PlayPunchClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(punchClips, spawnTransform, volume);
    }

    public void PlaySwordClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(swordClips, spawnTransform, volume);
    }

    public void PlayCriticalHitClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(criticalHitClips, spawnTransform, volume);
    }

    public void PlayTakeDamageClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(takeDamageClips, spawnTransform, volume);
    }

    public void PlayDeathClip(Transform spawnTransform, float volume = 0.5f)
    {
        PlayRandomClip(deathClips, spawnTransform, volume);
    }

    public void PlayLifeStealClip(Transform spawnTransform, float volume = 0.5f)
    {
        if (lifeStealClip != null)
        {
            PlaySoundFXClip(lifeStealClip, spawnTransform, volume);
        }
    }

    public void PlayBashClip(Transform spawnTransform, float volume = 0.5f)
    {
        if (bashClip != null)
        {
            PlaySoundFXClip(bashClip, spawnTransform, volume);
        }
    }
}
