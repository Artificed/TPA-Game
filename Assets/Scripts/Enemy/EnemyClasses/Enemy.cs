using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    private int _attack;
    private int _health;
    private int _defenseScalingFactor;
    private int _xpDrop;
    private GameObject _enemyPrefab;
    private bool _hasSword;
    private ArmorType _armorType;
    private Color _nameColor;
}
