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
    [SerializeField] private Color color;
    
    public void Initialize(string name, float currentHealth, float maxHealth, Color color)
    {
        if (enemyName != null)
        {
            enemyName.text = name;
            enemyName.color = color;
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

    public TextMeshProUGUI EnemyName
    {
        get => enemyName;
        set => enemyName = value;
    }
}
