using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class Enemy: MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int xpDrop;
    [SerializeField] private bool hasSword;
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
        health = Mathf.CeilToInt((data.health * levelMultiplier));
        maxHealth = health;
        defense = Mathf.CeilToInt((data.defense * levelMultiplier));
        xpDrop = Mathf.CeilToInt((data.xpDrop * levelMultiplier));
        
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
}
