using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalDamage;
    [SerializeField] private int exp;
    [SerializeField] private int expCap;
    [SerializeField] private int level;
    [SerializeField] private int zhen;

    [SerializeField] private PlayerDataSO playerDataSo;
    
    [SerializeField] private PlayerHealthEventChannel playerHealthEventChannel;
    [SerializeField] private PlayerExpEventChannel playerExpEventChannel;
    [SerializeField] private PlayerLevelEventChannel playerLevelEventChannel;
    [SerializeField] private PlayerFloorChangeEventChannel playerFloorChangeEventChannel;
    [SerializeField] private EnemyLeftEventChannel enemyLeftEventChannel;
    [SerializeField] private ZhenCounterEventChannel zhenCounterEventChannel;
    
    public static Player Instance { get; private set; } 
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Initialize(playerDataSo);
        DontDestroyOnLoad(gameObject); 
    }
    
    public void Initialize(PlayerDataSO data)
    {
        health = data.health;
        maxHealth = data.maxHealth;
        attack = data.attack;
        defense = data.defense;
        criticalRate = data.criticalRate;
        criticalDamage = data.criticalDamage;
        exp = data.exp;
        expCap = data.expCap;
        level = data.level;
        zhen = data.zhen;
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
    }
    
    public void HealHealth(int healthHealed)
    {
        health = Mathf.Clamp(health - healthHealed, 0, maxHealth);
        playerHealthEventChannel?.RaiseEvent(health, maxHealth);
    }

    public void AddExp(int expAdded)
    {
        exp += expAdded;
        if (exp > expCap)
        {
            level++;
            exp -= expCap;
            playerLevelEventChannel?.RaiseEvent(level);
            // TODO: Increase exp cap
        }
        playerExpEventChannel?.RaiseEvent(exp, expCap);
    }
}
