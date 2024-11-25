using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Enemy: MonoBehaviour
{
    [SerializeField] private string _enemyName;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _attack;
    [SerializeField] private int _defense;
    [SerializeField] private int _xpDrop;
    [SerializeField] private bool _hasSword;
    [SerializeField] private ArmorType _armorType;
    [SerializeField] private Color _color;
    
    [SerializeField] private EnemyUIController _enemyUIController;
    
    private void Start()
    {
        _enemyUIController.Initialize(_enemyName, _health, _maxHealth, _color);
        _enemyUIController.EnemyName.color = _color;
    }

    public void Initialize(EnemyDataSO data, string enemyName, int level)
    {
        float levelMultiplier = 1 + (level * 0.2f);
        
        _enemyName = enemyName;

        Random _random = new Random();
        
        _attack = Mathf.CeilToInt((data.attack * levelMultiplier) + _random.Next(-20, 21));
        _health = Mathf.CeilToInt((data.health * levelMultiplier) + _random.Next(-20, 21));
        _maxHealth = _health; 
        _defense = Mathf.CeilToInt((data.defense * levelMultiplier) + _random.Next(-20, 21));
        _xpDrop = Mathf.CeilToInt((data.xpDrop * levelMultiplier) + _random.Next(-20, 21));
        
        _hasSword = data.hasSword;
        _armorType = data.armorType;

        _color = data.nameColor;
    }
}
