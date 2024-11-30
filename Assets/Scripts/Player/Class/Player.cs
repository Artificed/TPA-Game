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
    
    [SerializeField] private SaveData fileData;
    
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
        // SaveSystem.LoadGameData();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (fileData.level > 1)
        {
            data.health = (int) CalculateStat(GetUpgradedHealth(), fileData.level, 1.20f);
            data.maxHealth = (int) CalculateStat(GetUpgradedHealth(), fileData.level, 1.20f);
            data.attack = (int) CalculateStat(GetUpgradedAttack(), fileData.level, 1.20f);
            data.defense = (int) CalculateStat(GetUpgradedDefense(), fileData.level, 1.20f);
            data.criticalRate = CalculateStat(GetUpgradedCriticalRate(), fileData.level, 1.10f);
            data.criticalDamage = CalculateStat(GetUpgradedCriticalDamage(), fileData.level, 1.20f);
            data.exp = fileData.exp;
            data.expCap = (int) CalculateStat(baseStats.expCap, fileData.level, 2f);
            data.level = fileData.level;
            data.zhen = fileData.zhen;
            data.floor = fileData.floor;
        }
        else
        {
            data.health = GetUpgradedHealth();
            data.maxHealth = GetUpgradedHealth();
            data.attack = GetUpgradedAttack();
            data.defense = GetUpgradedDefense();
            data.criticalRate = GetUpgradedCriticalRate();
            data.criticalDamage = GetUpgradedCriticalDamage();
            data.exp = baseStats.exp;
            data.expCap = baseStats.expCap;
            data.level = 1; 
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
        if (data.exp >= data.expCap)
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
        
        data.maxHealth = (int) CalculateStat(GetUpgradedHealth(), data.level, 1.20f);
        data.attack = (int) CalculateStat(GetUpgradedAttack(), data.level, 1.20f);
        data.defense = (int) CalculateStat(GetUpgradedDefense(), data.level, 1.20f);
        data.criticalRate = CalculateStat(GetUpgradedCriticalRate(), data.level, 1.20f);
        data.criticalDamage = CalculateStat(GetUpgradedCriticalDamage(), data.level, 1.20f);
        
        data.expCap = (int) CalculateStat(baseStats.expCap, data.level, 2.0f);
        
        playerHealthEventChannel?.RaiseEvent(data.health, data.maxHealth);
        playerExpEventChannel?.RaiseEvent(data.exp, data.expCap);
        playerLevelEventChannel?.RaiseEvent(data.level);
    }

    private float CalculateStat(float baseStat, int level, float scalingFactor)
    {
        if (level == 1) return baseStat;
        return baseStat * Mathf.Pow(scalingFactor, level - 1);
    }

    private int GetUpgradedHealth()
    {
        return baseStats.maxHealth + data.healthUpgradeLevel * 10;
    }
    
    private int GetUpgradedAttack()
    {
        return baseStats.attack + data.attackUpgradeLevel * 2;
    }

    private int GetUpgradedDefense()
    {
        return baseStats.defense + data.defenseUpgradeLevel * 5;
    }

    private float GetUpgradedCriticalRate()
    {
        return baseStats.criticalRate + data.criticalRateUpgradeLevel * 0.02f;
    }
    
    private float GetUpgradedCriticalDamage()
    {
        return baseStats.criticalDamage + data.criticalDamageUpgradeLevel * 0.05f;
    }
    
    public void SavePlayerData()
    {
        fileData.exp = data.exp;
        fileData.level = data.level;
        fileData.zhen = data.zhen;
        fileData.floor = data.floor;

        fileData.healthUpgradeLevel = data.healthUpgradeLevel;
        fileData.attackUpgradeLevel = data.attackUpgradeLevel;
        fileData.defenseUpgradeLevel = data.defenseUpgradeLevel;
        fileData.criticalRateUpgradeLevel = data.criticalRateUpgradeLevel;
        fileData.criticalDamageUpgradeLevel = data.criticalDamageUpgradeLevel;
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

    public SaveData FileData
    {
        get => fileData;
        set => fileData = value;
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
    public int HealthUpgradeLevel => data.healthUpgradeLevel;
    public int AttackUpgradeLevel => data.attackUpgradeLevel;
    public int DefenseUpgradeLevel => data.defenseUpgradeLevel;
    public int CriticalRateUpgradeLevel => data.criticalRateUpgradeLevel;
    public int CriticalDamageUpgradeLevel => data.criticalDamageUpgradeLevel;
}
