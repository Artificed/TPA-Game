using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Initialize(PlayerDataSO data)
    {
        health = data.health;
        maxHealth = data.maxHealth;
        attack = data.attack;
        defense = data.defense;
        criticalRate = data.criticalRate;
        criticalDamage = data.criticalDamage;
        exp = data.exp;
        expCap = data.expCap;
        level = data.level;
        zhen = data.zhen;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
