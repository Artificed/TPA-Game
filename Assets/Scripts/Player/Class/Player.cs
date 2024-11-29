using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalDamage;
    [SerializeField] private int exp;
    [SerializeField] private int expCap;
    [SerializeField] private int level;
    [SerializeField] private int zhen;
    [SerializeField] private int floor;
    
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
        if (data.level != 0)
        {
            health = data.maxHealth;
            maxHealth = data.maxHealth;
            attack = data.attack;
            defense = data.defense;
            criticalRate = data.criticalRate;
            criticalDamage = data.criticalDamage;
            exp = data.exp;
            expCap = data.expCap;
            level = data.level;
            zhen = data.zhen;
            floor = data.floor;
        }
        else
        {
            health = baseStats.maxHealth;
            maxHealth = baseStats.maxHealth;
            attack = baseStats.attack;
            defense = baseStats.defense;
            criticalRate = baseStats.criticalRate;
            criticalDamage = baseStats.criticalDamage;
            exp = baseStats.exp;
            expCap = baseStats.expCap;
            level = baseStats.level;
            zhen = baseStats.zhen;
            floor = baseStats.floor;
        }
        
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
        playerExpEventChannel?.RaiseEvent(exp, expCap);
        playerLevelEventChannel?.RaiseEvent(level);
        playerFloorChangeEventChannel?.RaiseEvent(floor);
        zhenCounterEventChannel?.RaiseEvent(zhen);
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
    }
    
    public void HealHealth(int healthHealed)
    {
        health = Mathf.Clamp(health + healthHealed, 0, maxHealth);
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
    }

    public void AddExp(int expAdded)
    {
        exp += expAdded;
        if (exp >= expCap)
        {
            LevelUp();
            PlayerStateMachine.Instance.showLevelUpText();
        }
        playerExpEventChannel?.RaiseEvent(exp, expCap);
    }

    public void AddZhen(int zhenAdded)
    {
        zhen += zhenAdded;
        zhenCounterEventChannel?.RaiseEvent(zhen);
    }

    public int Attack
    {
        get => attack;
        set => attack = value;
    }

    public void LevelUp()
    {
        level++;
        exp -= expCap;
        
        maxHealth += (int) ((maxHealth + Random.Range(0, 3)) * 0.25);
        attack += (int) ((attack + Random.Range(0, 3)) * 0.25);
        defense += (int) ((defense + Random.Range(0, 2)) * 0.25);
        criticalRate +=  (float) (criticalRate * 0.25);
        criticalDamage += (float) (criticalDamage * 0.25);
        expCap += (int) ((expCap + Random.Range(0, 3)) * 0.75);
        
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
        playerExpEventChannel?.RaiseEvent(exp, expCap);
        playerLevelEventChannel?.RaiseEvent(level);
    }

    public void SavePlayerData()
    {
        data.health = health;
        data.maxHealth = maxHealth;
        data.attack = attack;
        data.defense = defense;
        data.criticalRate = criticalRate;
        data.criticalDamage = criticalDamage;
        data.exp = exp;
        data.expCap = expCap;
        data.level = level;
        data.zhen = zhen;
        data.floor = floor;
    }

    public int Floor
    {
        get => floor;
        set => floor = value;
    }

    public int Health => health;
    public int Level => level;
    public int Defense => defense;
    public float CriticalRate => criticalRate;
    public float CriticalDamage => criticalDamage;
}
