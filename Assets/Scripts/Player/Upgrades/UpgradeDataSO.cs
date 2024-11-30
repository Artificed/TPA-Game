using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeDataSO : ScriptableObject
{
    public Texture2D imageIcon;
    public string itemName;
    public string description;
    public int currentCost;
    public int currentLevel;
    public int maxLevel;
    public int upgradeValue;

    public UnityAction OnDataChanged;
    
    public void SetLevel(int newLevel)
    {
        currentLevel = newLevel;
        OnDataChanged?.Invoke();
    }

    public void SetCost(int newCost)
    {
        currentCost = newCost;
        OnDataChanged?.Invoke();
    }
}
