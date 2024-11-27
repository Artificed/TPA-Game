using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public int attack;
    public int health;
    public int defense;
    public int xpDrop;
    public int zhenDrop;
    public bool hasSword;
    public float criticalRate;
    public float criticalDamage;
    public ArmorType armorType;
    public Color nameColor;
    public GameObject enemyPrefab;
    public EnemyType EnemyType;
}
