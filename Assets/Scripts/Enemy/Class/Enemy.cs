using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy: MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int xpDrop;
    [SerializeField] private int zhenDrop;
    [SerializeField] private bool hasSword;
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalDamage;
    [SerializeField] private ArmorType armorType;
    [SerializeField] private Color color;
    
    [SerializeField] private EnemyType enemyType;
    
    [SerializeField] private EnemyUIController enemyUIController;
    [SerializeField] private EnemyStateMachine enemyStateMachine;
    
    private void Start()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyUIController = GetComponent<EnemyUIController>();
        enemyUIController.Initialize(enemyName, health, maxHealth, color);
        enemyUIController.EnemyName.color = color;
    }

    public void Initialize(EnemyDataSO data, string enemyName, int level)
    {
        float levelMultiplier = 1 + (level * 0.2f);
        this.enemyName = enemyName;
        
        attack = Mathf.CeilToInt((data.attack * levelMultiplier)); 
        health = Mathf.CeilToInt((data.health * levelMultiplier) + Random.Range(0, 5));
        maxHealth = health;
        defense = Mathf.CeilToInt((data.defense * levelMultiplier) + Random.Range(0, 2));
        xpDrop = Mathf.CeilToInt((data.xpDrop * levelMultiplier) + Random.Range(0, 3));
        zhenDrop = Mathf.CeilToInt((data.zhenDrop * levelMultiplier) + Random.Range(0, 3));
        
        criticalRate = data.criticalRate;
        criticalDamage = data.criticalDamage;
        
        hasSword = data.hasSword;
        armorType = data.armorType;

        color = data.nameColor;
        enemyType = data.EnemyType;
    }

    public EnemyStateMachine EnemyStateMachine
    {
        get => enemyStateMachine;
        set => enemyStateMachine = value;
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        enemyUIController.UpdateHealthBar(health, maxHealth);
    }

    public EnemyType EnemyType => enemyType;
    public int Attack => attack;
    public int Health => health;
    public int Defense => defense;
    public float CriticalRate => criticalRate;
    public float CriticalDamage => criticalDamage;
    public int XpDrop => xpDrop;
    public int ZhenDrop => zhenDrop;
}
