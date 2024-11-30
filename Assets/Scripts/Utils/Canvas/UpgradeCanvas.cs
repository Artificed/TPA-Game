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
    [SerializeField] private GameObject errorText;

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
    
    [SerializeField] private UpgradeDataEventChannel upgradeDataEventChannel;
    
    void Start()
    {
        zhenCounter.text = playerDataSo.zhen.ToString();
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
        if (playerDataSo.zhen < currentUpgradeSo.currentCost)
        {
            errorText.SetActive(true);
        }
        else
        {
            errorText.SetActive(false);
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
        }
        if (currentUpgradeSo.itemName.Equals("Health Up"))
        {
            playerDataSo.maxHealth += currentUpgradeSo.upgradeValue;
            playerUpgradesSo.healthUpgradeLevel++;
            playerUpgradesSo.healthUpgradeCost = currentUpgradeSo.currentCost;
        } 
        if (currentUpgradeSo.itemName.Equals("Defense Up"))
        {
            playerDataSo.defense += currentUpgradeSo.upgradeValue;
            playerUpgradesSo.defenseUpgradeLevel++;
            playerUpgradesSo.defenseUpgradeCost = currentUpgradeSo.currentCost;
        } 
        if (currentUpgradeSo.itemName.Equals("Luck Up"))
        {
            playerDataSo.criticalRate += (float) currentUpgradeSo.upgradeValue / 100;
            playerUpgradesSo.criticalChanceUpgradeLevel++;
            playerUpgradesSo.criticalChanceUpgradeCost = currentUpgradeSo.currentCost;
        } 
        if (currentUpgradeSo.itemName.Equals("Crit Dmg Up"))
        {
            playerDataSo.criticalDamage += (float) currentUpgradeSo.upgradeValue / 100;
            playerUpgradesSo.criticalDamageUpgradeLevel++;
            playerUpgradesSo.criticalDamageUpgradeCost = currentUpgradeSo.currentCost;
        }
        upgradeDataEventChannel.RaiseEvent(currentUpgradeSo);
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
            return "Current   :  " + playerUpgradesSo.criticalDamageUpgradeLevel * 100 + "% crit dmg";
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
        upgradeDataEventChannel.onIconSelected.AddListener(HandleUpgradeIconClick);
    }
    
    private void OnDisable()
    {
        upgradeDataEventChannel.onIconSelected.RemoveListener(HandleUpgradeIconClick);
    }
}
