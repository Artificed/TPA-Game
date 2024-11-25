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
        Random _random = new Random();
        
        switch (enemyType)
        {
            case EnemyType.Common:
                attack = Mathf.CeilToInt((data.attack * levelMultiplier) + _random.Next(2)); 
                health = Mathf.CeilToInt((data.health * levelMultiplier) + _random.Next(3));
                maxHealth = health;
                defense = Mathf.CeilToInt((data.defense * levelMultiplier) + _random.Next(2));
                xpDrop = Mathf.CeilToInt((data.xpDrop * levelMultiplier) + _random.Next(3));
                break;
        
            case EnemyType.Medium:
                attack = Mathf.CeilToInt((data.attack * levelMultiplier) + _random.Next(6)); 
                health = Mathf.CeilToInt((data.health * levelMultiplier) + _random.Next(20));
                maxHealth = health;
                defense = Mathf.CeilToInt((data.defense * levelMultiplier) + _random.Next(4));
                xpDrop = Mathf.CeilToInt((data.xpDrop * levelMultiplier) + _random.Next(8));
                break;
        
            case EnemyType.Elite:
                attack = Mathf.CeilToInt((data.attack * levelMultiplier) + _random.Next(10)); 
                health = Mathf.CeilToInt((data.health * levelMultiplier) + _random.Next(50));
                maxHealth = health;
                defense = Mathf.CeilToInt((data.defense * levelMultiplier) + _random.Next(10));
                xpDrop = Mathf.CeilToInt((data.xpDrop * levelMultiplier) + _random.Next(20));
                break;
        }
        
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

    public int Health => health;
}
