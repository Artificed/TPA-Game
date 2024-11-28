using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private PlayerDeathEventChannel playerDeathEventChannel;
    [SerializeField] private GameObject deathCanvas;
    
    public void HandlePlayerDeath()
    {
        Debug.Log("reached");
        deathCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        playerDeathEventChannel.PlayerDeathEvent += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        playerDeathEventChannel.PlayerDeathEvent -= HandlePlayerDeath;
    }
}
