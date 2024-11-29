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
    
    [SerializeField] public int healthUpgradeLevel;
    [SerializeField] public int attackUpgradeLevel;
    [SerializeField] public int defenseUpgradeLevel;
    [SerializeField] public int criticalRateUpgradeLevel;
    [SerializeField] public int criticalDamageUpgradeLevel;
    
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
        if (data.level > 1)
        {
            health = (int) CalculateStat(GetUpgradedHealth(), data.level, 1.20f);
            maxHealth = (int) CalculateStat(GetUpgradedHealth(), data.level, 1.20f);
            attack = (int) CalculateStat(GetUpgradedAttack(), data.level, 1.20f);
            defense = (int) CalculateStat(GetUpgradedDefense(), data.level, 1.20f);
            criticalRate = CalculateStat(GetUpgradedCriticalRate(), data.level, 1.10f);
            criticalDamage = CalculateStat(GetUpgradedCriticalDamage(), data.level, 1.20f);
            exp = data.exp;
            expCap = (int) CalculateStat(baseStats.expCap, data.level, 2f);
            level = data.level;
            zhen = data.zhen;
            floor = data.floor;
        }
        else
        {
            health = GetUpgradedHealth();
            maxHealth = GetUpgradedHealth();
            attack = GetUpgradedAttack();
            defense = GetUpgradedDefense();
            criticalRate = GetUpgradedCriticalRate();
            criticalDamage = GetUpgradedCriticalDamage();
            exp = baseStats.exp;
            expCap = baseStats.expCap;
            level = 1; 
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
        
        maxHealth = (int) CalculateStat(GetUpgradedHealth(), level, 1.20f);
        attack = (int) CalculateStat(GetUpgradedAttack(), level, 1.20f);
        defense = (int) CalculateStat(GetUpgradedDefense(), level, 1.20f);
        criticalRate = CalculateStat(GetUpgradedCriticalRate(), level, 1.20f);
        criticalDamage = CalculateStat(GetUpgradedCriticalDamage(), level, 1.20f);
        
        expCap = (int) CalculateStat(baseStats.expCap, level, 2.0f);
        
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
        playerExpEventChannel?.RaiseEvent(exp, expCap);
        playerLevelEventChannel?.RaiseEvent(level);
    }

    private float CalculateStat(float baseStat, int level, float scalingFactor)
    {
        if (level == 1) return baseStat;
        return baseStat * Mathf.Pow(scalingFactor, level - 1);
    }

    private int GetUpgradedHealth()
    {
        return baseStats.maxHealth + healthUpgradeLevel * 10;
    }
    
    private int GetUpgradedAttack()
    {
        return baseStats.attack + attackUpgradeLevel * 2;
    }

    private int GetUpgradedDefense()
    {
        return baseStats.defense + defenseUpgradeLevel * 5;
    }

    private float GetUpgradedCriticalRate()
    {
        return baseStats.criticalRate + criticalRateUpgradeLevel * 0.02f;
    }
    
    private float GetUpgradedCriticalDamage()
    {
        return baseStats.criticalDamage + criticalDamageUpgradeLevel * 0.05f;
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

        data.healthUpgradeLevel = healthUpgradeLevel;
        data.attackUpgradeLevel = attackUpgradeLevel;
        data.defenseUpgradeLevel = defenseUpgradeLevel;
        data.criticalRateUpgradeLevel = criticalRateUpgradeLevel;
        data.criticalDamageUpgradeLevel = criticalDamageUpgradeLevel;
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
