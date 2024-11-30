using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
