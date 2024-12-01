using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitToMainButton : ButtonHandler
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;

    void Start()
    {
        button.onClick.AddListener(HandleExit);
    }
    
    public void HandleExit()
    {
        audioSource.PlayOneShot(normalClickSound);
        SaveSystem.SaveGameData(playerDataSo, playerUpgradesSo);
        SceneManager.LoadScene("MainMenu");
    }
}
