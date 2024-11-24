using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public int attack;
    public int health;
    public int defenseScalingFactor;
    public int xpDrop;
    public bool hasSword;
    public ArmorType armorType;
    public Color nameColor;
    public GameObject enemyPrefab;
}
