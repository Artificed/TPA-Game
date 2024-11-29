using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float criticalRate;
    public float criticalDamage;
    public int exp;
    public int expCap;
    public int level;
    public int zhen;
    public int floor;

    public int healthUpgradeLevel;
    public int attackUpgradeLevel;
    public int defenseUpgradeLevel;
    public int criticalRateUpgradeLevel;
    public int criticalDamageUpgradeLevel;
}