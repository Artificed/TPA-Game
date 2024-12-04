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

    public int selectedFloor;
    
    public void Reset()
    {
        health = 20;
        maxHealth = 20;
        attack = 5;
        defense = 5;
        criticalRate = 0.05f;
        criticalDamage = 1.5f;
        exp = 0;
        expCap = 5;
        level = 1;
        zhen = 0;
        floor = 1;
        selectedFloor = 1;
    }

    public void IncreaseExp(int amount)
    {
        exp += amount;
        while (exp >= expCap)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        exp -= expCap;
        
        maxHealth = (int) (maxHealth * 1.20f);
        attack = (int) (attack * 1.20f);
        defense = (int) (defense * 1.20f);
        criticalRate = (criticalRate * 1.05f);
        criticalDamage = (criticalDamage * 1.05f);
        
        expCap = (int) (expCap * 1.3f);
    }
}