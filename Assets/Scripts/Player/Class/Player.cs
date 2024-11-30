using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerDataSO baseStats;
    [SerializeField] private PlayerDataSO data;
    
    [SerializeField] private PlayerHealthEventChannel playerHealthEventChannel;
    [SerializeField] private PlayerExpEventChannel playerExpEventChannel;
    [SerializeField] private PlayerLevelEventChannel playerLevelEventChannel;
    [SerializeField] private PlayerFloorChangeEventChannel playerFloorChangeEventChannel;
    [SerializeField] private EnemyLeftEventChannel enemyLeftEventChannel;
    [SerializeField] private ZhenCounterEventChannel zhenCounterEventChannel;
    
    public static Player Instance { get; private set; } 
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (data.level < 2)
        {
            data.health = baseStats.health;
            data.maxHealth = baseStats.maxHealth;
            data.attack = baseStats.attack;
            data.defense = baseStats.defense;
            data.criticalRate = baseStats.criticalRate;
            data.criticalDamage = baseStats.criticalDamage;
            data.exp = baseStats.exp;
            data.expCap = baseStats.expCap;
            data.level = baseStats.level;
            data.zhen = baseStats.zhen;
            data.floor = baseStats.floor;
        }
        
        playerHealthEventChannel?.RaiseEvent(data.health, data.maxHealth);
        playerExpEventChannel?.RaiseEvent(data.exp, data.expCap);
        playerLevelEventChannel?.RaiseEvent(data.level);
        playerFloorChangeEventChannel?.RaiseEvent(data.floor);
        zhenCounterEventChannel?.RaiseEvent(data.zhen);
    }

    public void TakeDamage(int damage)
    {
        data.health = Mathf.Clamp(data.health - damage, 0, data.maxHealth);
        playerHealthEventChannel?.RaiseEvent(data.health, data.maxHealth);
    }
    
    public void HealHealth(int healthHealed)
    {
        data.health = Mathf.Clamp(data.health + healthHealed, 0, data.maxHealth);
        playerHealthEventChannel?.RaiseEvent(data.health, data.maxHealth);
    }

    public void AddExp(int expAdded)
    {
        data.exp += expAdded;
        while (data.exp >= data.expCap)
        {
            LevelUp();
            PlayerStateMachine.Instance.showLevelUpText();
        }
        playerExpEventChannel?.RaiseEvent(data.exp, data.expCap);
    }

    public void AddZhen(int zhenAdded)
    {
        data.zhen += zhenAdded;
        zhenCounterEventChannel?.RaiseEvent(data.zhen);
    }

    public void LevelUp()
    {
        data.level++;
        data.exp -= data.expCap;
        
        data.maxHealth = (int) (data.maxHealth * 1.20f);
        data.attack = (int) (data.attack * 1.20f);
        data.defense = (int) (data.defense * 1.20f);
        data.criticalRate = (data.criticalRate * 1.20f);
        data.criticalDamage = (data.criticalDamage * 1.20f);
        
        data.expCap = (int) (data.expCap * 2.0f);
        
        playerHealthEventChannel?.RaiseEvent(data.health, data.maxHealth);
        playerExpEventChannel?.RaiseEvent(data.exp, data.expCap);
        playerLevelEventChannel?.RaiseEvent(data.level);
    }

    public int Attack
    {
        get => data.attack;
        set => data.attack = value;
    }
    
    public int Floor
    {
        get => data.floor;
        set => data.floor = value;
    }

    public int Health => data.health;
    public int MaxHealth => data.maxHealth;
    public int Defense => data.defense;
    public float CriticalRate => data.criticalRate;
    public float CriticalDamage => data.criticalDamage;
    public int Exp => data.exp;
    public int ExpCap => data.expCap;
    public int Level => data.level;
    public int Zhen => data.zhen;
}
