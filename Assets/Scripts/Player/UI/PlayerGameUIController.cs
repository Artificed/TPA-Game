using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameUIController : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI expText;
    
    [SerializeField] private int level;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI zhenCountText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private TextMeshProUGUI enemyLeftText;
    
    [SerializeField] private PlayerHealthEventChannel playerHealthEventChannel;
    [SerializeField] private PlayerExpEventChannel playerExpEventChannel;
    [SerializeField] private PlayerLevelEventChannel playerLevelEventChannel;
    [SerializeField] private ZhenCounterEventChannel zhenCounterEventChannel;
    [SerializeField] private PlayerFloorChangeEventChannel floorChangeEventChannel;
    [SerializeField] private EnemyLeftEventChannel enemyLeftEventChannel;
    
    private void UpdateHealthUI(int health, int maxHealth)
    {
        healthText.text = health + "/" + maxHealth;
        hpSlider.value = (float) health / (float) maxHealth;
    }

    private void UpdateXpUI(int exp, int expCap)
    {
        expText.text = exp + "/" + expCap;
        expSlider.value = (float) exp / (float) expCap;
    }

    private void UpdateLevelUI(int currentLevel)
    {
        levelText.text = "Level: " + currentLevel;
    }

    private void UpdateZhenUI(int zhen)
    {
        zhenCountText.text = zhen.ToString();
    }

    private void UpdateFloorCountUI(int floor)
    {
        floorText.text = "Floor: " + floor;
    }
    
    private void UpdateEnemyLeftUI(int enemyLeft)
    {
        enemyLeftText.text = "Enemy Left " + enemyLeft;
    }
    
    private void OnEnable()
    {
        playerHealthEventChannel.OnHealthChanged += UpdateHealthUI;
        playerExpEventChannel.OnExpChanged += UpdateXpUI;
        playerLevelEventChannel.OnLevelChanged += UpdateLevelUI;
        zhenCounterEventChannel.OnZhenChanged += UpdateZhenUI;
        floorChangeEventChannel.OnFloorChanged += UpdateFloorCountUI;
        enemyLeftEventChannel.OnEnemyLeftChanged += UpdateEnemyLeftUI;
    }
    
    private void OnDisable()
    {
        playerHealthEventChannel.OnHealthChanged -= UpdateHealthUI;
        playerExpEventChannel.OnExpChanged -= UpdateXpUI;
        playerLevelEventChannel.OnLevelChanged -= UpdateLevelUI;
        zhenCounterEventChannel.OnZhenChanged -= UpdateZhenUI;
        floorChangeEventChannel.OnFloorChanged -= UpdateFloorCountUI;
        enemyLeftEventChannel.OnEnemyLeftChanged -= UpdateEnemyLeftUI;
    }
}
