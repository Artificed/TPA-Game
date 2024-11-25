using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class EnemyUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private Slider healthBar;
    
    public void Initialize(string name, float currentHealth, float maxHealth)
    {
        if (enemyName != null)
        {
            enemyName.text = name;
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth /  maxHealth;
        }
    }
    
    
}
