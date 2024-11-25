using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy: MonoBehaviour
{
    [SerializeField] private string _enemyName;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _attack;
    [SerializeField] private int _defenseScalingFactor;
    [SerializeField] private int _xpDrop;
    [SerializeField] private bool _hasSword;
    [SerializeField] private ArmorType _armorType;
    
    private EnemyUIController _enemyUIController;
    
    private void Start()
    {
        _enemyUIController = GetComponentInChildren<EnemyUIController>();
        _enemyUIController.Initialize(_enemyName, _health, _maxHealth);
    }

    public void Initialize(EnemyDataSO data, string enemyName)
    {
        _enemyName = enemyName;
        _attack = data.attack;
        _health = data.health;
        _maxHealth = data.health;
        _defenseScalingFactor = data.defenseScalingFactor;
        _xpDrop = data.xpDrop;
        _hasSword = data.hasSword;
        _armorType = data.armorType;
    }
}
