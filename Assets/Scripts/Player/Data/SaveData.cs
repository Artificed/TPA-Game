using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int exp;
    public int level;
    public int zhen;
    public int floor;

    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float criticalRate;
    public float criticalDamage;
    public int expCap;
    
    public int healthUpgradeLevel;
    public int attackUpgradeLevel;
    public int defenseUpgradeLevel;
    public int criticalChanceUpgradeLevel;
    public int criticalDamageUpgradeLevel;
    
    public int healthUpgradeCost;
    public int attackUpgradeCost;
    public int defenseUpgradeCost;
    public int criticalChanceUpgradeCost;
    public int criticalDamageUpgradeCost;
    
    public SaveData(PlayerDataSO playerData, PlayerUpgradesSO playerUpgrades)
    {
        this.exp = playerData.exp;
        this.level = playerData.level;
        this.zhen = playerData.zhen;
        this.floor = playerData.floor;

        this.health = playerData.health;
        this.maxHealth = playerData.maxHealth;
        this.attack = playerData.attack;
        this.defense = playerData.defense;
        this.criticalRate = playerData.criticalRate;
        this.criticalDamage = playerData.criticalDamage;
        this.expCap = playerData.expCap;

        this.healthUpgradeLevel = playerUpgrades.healthUpgradeLevel;
        this.attackUpgradeLevel = playerUpgrades.attackUpgradeLevel;
        this.defenseUpgradeLevel = playerUpgrades.defenseUpgradeLevel;
        this.criticalChanceUpgradeLevel = playerUpgrades.criticalChanceUpgradeLevel;
        this.criticalDamageUpgradeLevel = playerUpgrades.criticalDamageUpgradeLevel;

        this.healthUpgradeCost = playerUpgrades.healthUpgradeCost;
        this.attackUpgradeCost = playerUpgrades.attackUpgradeCost;
        this.defenseUpgradeCost = playerUpgrades.defenseUpgradeCost;
        this.criticalChanceUpgradeCost = playerUpgrades.criticalChanceUpgradeCost;
        this.criticalDamageUpgradeCost = playerUpgrades.criticalDamageUpgradeCost;
    }

    public void TransferData(PlayerDataSO playerData, PlayerUpgradesSO playerUpgrades)
    {
        playerData.exp = this.exp;
        playerData.level = this.level;
        playerData.zhen = this.zhen;
        playerData.floor = this.floor;

        playerData.health = this.health;
        playerData.maxHealth = this.maxHealth;
        playerData.attack = this.attack;
        playerData.defense = this.defense;
        playerData.criticalRate = this.criticalRate;
        playerData.criticalDamage = this.criticalDamage;
        playerData.expCap = this.expCap;

        playerUpgrades.healthUpgradeLevel = this.healthUpgradeLevel;
        playerUpgrades.attackUpgradeLevel = this.attackUpgradeLevel;
        playerUpgrades.defenseUpgradeLevel = this.defenseUpgradeLevel;
        playerUpgrades.criticalChanceUpgradeLevel = this.criticalChanceUpgradeLevel;
        playerUpgrades.criticalDamageUpgradeLevel = this.criticalDamageUpgradeLevel;

        playerUpgrades.healthUpgradeCost = this.healthUpgradeCost;
        playerUpgrades.attackUpgradeCost = this.attackUpgradeCost;
        playerUpgrades.defenseUpgradeCost = this.defenseUpgradeCost;
        playerUpgrades.criticalChanceUpgradeCost = this.criticalChanceUpgradeCost;
        playerUpgrades.criticalDamageUpgradeCost = this.criticalDamageUpgradeCost;
    }

}
