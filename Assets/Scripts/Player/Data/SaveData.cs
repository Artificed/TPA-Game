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

    public int healthUpgradeLevel;
    public int attackUpgradeLevel;
    public int defenseUpgradeLevel;
    public int criticalRateUpgradeLevel;
    public int criticalDamageUpgradeLevel;
    
    public int healthUpgradeCost;
    public int attackUpgradeCost;
    public int defenseUpgradeCost;
    public int criticalRateUpgradeCost;
    public int criticalDamageUpgradeCost;
    
    public SaveData(Player player)
    {
        this.exp = player.Exp;
        this.level = player.Level;
        this.zhen = player.Zhen;
        this.floor = player.Floor;

        this.healthUpgradeLevel = player.HealthUpgradeLevel;
        this.attackUpgradeLevel = player.AttackUpgradeLevel;
        this.defenseUpgradeLevel = player.DefenseUpgradeLevel;
        this.criticalRateUpgradeLevel = player.CriticalRateUpgradeLevel;
        this.criticalDamageUpgradeLevel = player.CriticalDamageUpgradeLevel;
    }
}
