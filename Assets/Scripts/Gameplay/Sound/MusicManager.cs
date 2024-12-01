using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicObject;
    [SerializeField] private AudioClip battleTheme;
    [SerializeField] private AudioClip dungeonTheme;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlayDungeonTheme();
    }

    private void Update()
    {
        CheckSwitchThemes();
    }

    public void CheckSwitchThemes()
    {
        if (TurnManager.Instance.AggroedEnemies.Count > 0 && musicObject.clip != battleTheme)
        {
            PlayBattleTheme();
        }
        else if (TurnManager.Instance.AggroedEnemies.Count == 0 && musicObject.clip != dungeonTheme)
        {
            PlayDungeonTheme();
        }
    }

    public void PlayBattleTheme()
    {
        musicObject.clip = battleTheme;
        musicObject.Play();
    }

    public void PlayDungeonTheme()
    {
        musicObject.clip = dungeonTheme;
        musicObject.Play();
    }
}
