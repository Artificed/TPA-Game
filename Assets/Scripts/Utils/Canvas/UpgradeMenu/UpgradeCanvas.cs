using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI zhenCounter;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemCurrentStat;
    [SerializeField] private TextMeshProUGUI itemUpgradeStat;
    [SerializeField] private GameObject bottomZhenLogo;
    [SerializeField] private TextMeshProUGUI itemUpgradeCost;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private ButtonHandler upgradeButtonHandler;
    
    [SerializeField] private TextMeshProUGUI cheatCodeTextField;
    
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;

    [SerializeField] private UpgradeDataSO attackUpgradeSo;
    [SerializeField] private UpgradeDataSO healthUpgradeSo;
    [SerializeField] private UpgradeDataSO defenseUpgradeSo;
    [SerializeField] private UpgradeDataSO criticalChanceUpgradeSo;
    [SerializeField] private UpgradeDataSO criticalDamageUpgradeSo;
    
    [SerializeField] private UpgradeDataSO currentUpgradeSo;
    
    [SerializeField] private UpgradeDataEventChannel healthUpgradeDataEventChannel;
    [SerializeField] private UpgradeDataEventChannel attackUpgradeDataEventChannel;
    [SerializeField] private UpgradeDataEventChannel defenseUpgradeDataEventChannel;
    [SerializeField] private UpgradeDataEventChannel criticalChanceUpgradeDataEventChannel;
    [SerializeField] private UpgradeDataEventChannel criticalDamageUpgradeDataEventChannel;
    
    void Start()
    {
        UpdateUpgradeData();
        zhenCounter.text = playerDataSo.zhen.ToString();
    }

    private void UpdateUpgradeData()
    {
        attackUpgradeSo.SetCost(playerUpgradesSo.attackUpgradeCost);
        attackUpgradeSo.SetLevel(playerUpgradesSo.attackUpgradeLevel);
        
        healthUpgradeSo.SetCost(playerUpgradesSo.healthUpgradeCost);
        healthUpgradeSo.SetLevel(playerUpgradesSo.healthUpgradeLevel);
        
        defenseUpgradeSo.SetCost(playerUpgradesSo.defenseUpgradeCost);
        defenseUpgradeSo.SetLevel(playerUpgradesSo.defenseUpgradeLevel);
        
        criticalChanceUpgradeSo.SetCost(playerUpgradesSo.criticalChanceUpgradeCost);
        criticalChanceUpgradeSo.SetLevel(playerUpgradesSo.criticalChanceUpgradeLevel);
        
        criticalDamageUpgradeSo.SetCost(playerUpgradesSo.criticalDamageUpgradeCost);
        criticalDamageUpgradeSo.SetLevel(playerUpgradesSo.criticalDamageUpgradeLevel);
    }

    private void HandleUpgradeIconClick(UpgradeDataSO upgradeData)
    {
        currentUpgradeSo = upgradeData;

        itemImage.color = Color.white;
        itemImage.sprite = Sprite.Create(upgradeData.imageIcon, 
            new Rect(0, 0, upgradeData.imageIcon.width, upgradeData.imageIcon.height), new Vector2(0.5f, 0.5f));

        itemName.text = upgradeData.itemName;
        itemDescription.text = upgradeData.description;
        itemCurrentStat.text = GetPlayerCurrentStat();
        itemUpgradeStat.text = GetPlayerUpgradeStat();
        itemUpgradeCost.text = currentUpgradeSo.currentCost + " To upgrade";
        
        bottomZhenLogo.SetActive(true);
        upgradeButton.SetActive(true);
    }

    public void HandleUpgradeClick()
    {
        if (currentUpgradeSo.currentLevel >= currentUpgradeSo.maxLevel)
        {
            errorText.text = "Max Level Reached!";
        }
        else if (playerDataSo.zhen < currentUpgradeSo.currentCost)
        {
            errorText.text = "You don't have enough Zhen!";
        }
        else
        {
            errorText.text = "";
            HandlePurchase();
        }
    }

    private void HandlePurchase()
    {
        upgradeButtonHandler.UsePurchaseSound();
        playerDataSo.zhen -= currentUpgradeSo.currentCost;
        zhenCounter.text = playerDataSo.zhen.ToString();
        
        currentUpgradeSo.SetLevel(currentUpgradeSo.currentLevel + 1);
        IncreaseAllCosts();
        currentUpgradeSo.SetCost(currentUpgradeSo.currentCost + 40);
        
        if (currentUpgradeSo.itemName.Equals("Attack Up"))
        {
            playerDataSo.attack += currentUpgradeSo.upgradeValue;
            playerUpgradesSo.attackUpgradeLevel++;
            playerUpgradesSo.attackUpgradeCost = currentUpgradeSo.currentCost;
            attackUpgradeDataEventChannel.RaiseEvent(currentUpgradeSo);
        }
        if (currentUpgradeSo.itemName.Equals("Health Up"))
        {
            playerDataSo.maxHealth += currentUpgradeSo.upgradeValue;
            playerUpgradesSo.healthUpgradeLevel++;
            playerUpgradesSo.healthUpgradeCost = currentUpgradeSo.currentCost;
            healthUpgradeDataEventChannel.RaiseEvent(currentUpgradeSo);
        } 
        if (currentUpgradeSo.itemName.Equals("Defense Up"))
        {
            playerDataSo.defense += currentUpgradeSo.upgradeValue;
            playerUpgradesSo.defenseUpgradeLevel++;
            playerUpgradesSo.defenseUpgradeCost = currentUpgradeSo.currentCost;
            defenseUpgradeDataEventChannel.RaiseEvent(currentUpgradeSo);
        } 
        if (currentUpgradeSo.itemName.Equals("Luck Up"))
        {
            playerDataSo.criticalRate += (float) currentUpgradeSo.upgradeValue / 100;
            playerUpgradesSo.criticalChanceUpgradeLevel++;
            playerUpgradesSo.criticalChanceUpgradeCost = currentUpgradeSo.currentCost;
            criticalChanceUpgradeDataEventChannel.RaiseEvent(currentUpgradeSo);
        } 
        if (currentUpgradeSo.itemName.Equals("Crit Dmg Up"))
        {
            playerDataSo.criticalDamage += (float) currentUpgradeSo.upgradeValue / 100;
            playerUpgradesSo.criticalDamageUpgradeLevel++;
            playerUpgradesSo.criticalDamageUpgradeCost = currentUpgradeSo.currentCost;
            criticalDamageUpgradeDataEventChannel.RaiseEvent(currentUpgradeSo);
        }
    }

    private void IncreaseAllCosts()
    {
        healthUpgradeSo.currentCost += 10;
        attackUpgradeSo.currentCost += 10;
        defenseUpgradeSo.currentCost += 10;
        criticalChanceUpgradeSo.currentCost += 10;
        criticalDamageUpgradeSo.currentCost += 10;

        playerUpgradesSo.healthUpgradeCost = healthUpgradeSo.currentCost;
        playerUpgradesSo.attackUpgradeCost = attackUpgradeSo.currentCost;
        playerUpgradesSo.defenseUpgradeCost = defenseUpgradeSo.currentCost;
        playerUpgradesSo.criticalChanceUpgradeCost = criticalChanceUpgradeSo.currentCost;
        playerUpgradesSo.criticalDamageUpgradeCost = criticalDamageUpgradeSo.currentCost;
    }
    
    private string GetPlayerCurrentStat()
    {
        if (currentUpgradeSo.itemName.Equals("Attack Up"))
        {
            return "Current   :  " + playerDataSo.attack + " atk";
        }
        if (currentUpgradeSo.itemName.Equals("Health Up"))
        {
            return "Current   :  " + playerDataSo.maxHealth + " hp";
        } 
        if (currentUpgradeSo.itemName.Equals("Defense Up"))
        {
            return "Current   :  " + playerDataSo.defense + " def";
        } 
        if (currentUpgradeSo.itemName.Equals("Luck Up"))
        {
            return "Current   :  " + playerDataSo.criticalRate * 100 + "% crit rate";
        } 
        if (currentUpgradeSo.itemName.Equals("Crit Dmg Up"))
        {
            return "Current   :  " + playerDataSo.criticalDamage * 100 + "% crit dmg";
        }
        return "";
    }

    private string GetPlayerUpgradeStat()
    {
        if (currentUpgradeSo.itemName.Equals("Attack Up"))
        {
            return "Upgrade :  +" + currentUpgradeSo.upgradeValue + " atk";
        }
        if (currentUpgradeSo.itemName.Equals("Health Up"))
        {
            return "Upgrade :  +" + currentUpgradeSo.upgradeValue + " hp";
        } 
        if (currentUpgradeSo.itemName.Equals("Defense Up"))
        {
            return "Upgrade :  +" + currentUpgradeSo.upgradeValue + " def";
        } 
        if (currentUpgradeSo.itemName.Equals("Luck Up"))
        {
            return "Upgrade :  +" + currentUpgradeSo.upgradeValue + "% crit rate";
        } 
        if (currentUpgradeSo.itemName.Equals("Crit Dmg Up"))
        {
            return "Upgrade :  +" + currentUpgradeSo.upgradeValue + "% crit dmg";
        }
        return "";
    }

    private void OnEnable()
    {
        attackUpgradeDataEventChannel.onIconSelected.AddListener(HandleUpgradeIconClick);
        healthUpgradeDataEventChannel.onIconSelected.AddListener(HandleUpgradeIconClick);
        defenseUpgradeDataEventChannel.onIconSelected.AddListener(HandleUpgradeIconClick);
        criticalChanceUpgradeDataEventChannel.onIconSelected.AddListener(HandleUpgradeIconClick);
        criticalDamageUpgradeDataEventChannel.onIconSelected.AddListener(HandleUpgradeIconClick);
    }
    
    private void OnDisable()
    {
        attackUpgradeDataEventChannel.onIconSelected.RemoveListener(HandleUpgradeIconClick);
        healthUpgradeDataEventChannel.onIconSelected.RemoveListener(HandleUpgradeIconClick);
        defenseUpgradeDataEventChannel.onIconSelected.RemoveListener(HandleUpgradeIconClick);
        criticalChanceUpgradeDataEventChannel.onIconSelected.RemoveListener(HandleUpgradeIconClick);
        criticalDamageUpgradeDataEventChannel.onIconSelected.RemoveListener(HandleUpgradeIconClick);
    }
}
