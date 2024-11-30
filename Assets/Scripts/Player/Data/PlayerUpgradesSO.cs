using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradesSO", menuName = "ScriptableObjects/PlayerUpgradesSO")]
public class PlayerUpgradesSO : ScriptableObject
{
    public int healthUpgradeLevel;
    public int healthUpgradeCost;
    public int attackUpgradeLevel;
    public int attackUpgradeCost;
    public int defenseUpgradeLevel;
    public int defenseUpgradeCost;
    public int criticalChanceUpgradeLevel;
    public int criticalChanceUpgradeCost;
    public int criticalDamageUpgradeLevel;
    public int criticalDamageUpgradeCost;
}
