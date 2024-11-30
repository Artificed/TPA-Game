using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvas : MonoBehaviour
{
    [SerializeField] private int zhenCounter;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemCurrentStat;
    [SerializeField] private TextMeshProUGUI itemUpgradeStat;
    [SerializeField] private GameObject bottomZhenLogo;
    [SerializeField] private TextMeshProUGUI itemUpgradeCost;
    [SerializeField] private GameObject upgradeButton;

    [SerializeField] private UpgradeDataSO currentUpgradeSo;

    [SerializeField] private UpgradeDataEventChannel upgradeDataEventChannel;
    
    void Start()
    {
        // zhenCounter = Player.Instance.Zhen;
    }

    private void HandleUpgradeIconClick(UpgradeDataSO data)
    {
        currentUpgradeSo = data;
        Debug.Log(data.itemName);

        itemImage.color = Color.white;
        itemImage.sprite = Sprite.Create(data.imageIcon, 
            new Rect(0, 0, data.imageIcon.width, data.imageIcon.height), new Vector2(0.5f, 0.5f));

        itemName.text = data.itemName;
        itemDescription.text = data.description;
        itemCurrentStat.text = "Current   : " + data.currentLevel;
        itemUpgradeStat.text = "Upgrade : " + data.upgradeValue;
        itemUpgradeCost.text = data.currentCost + " To upgrade";
        
        bottomZhenLogo.SetActive(true);
        upgradeButton.SetActive(true);
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
