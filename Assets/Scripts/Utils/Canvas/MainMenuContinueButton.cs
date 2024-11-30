using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuContinueButton : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;
    
    public void HandleContinue()
    {
        SaveData saveData = SaveSystem.LoadGameData();
        saveData.TransferData(playerDataSo, playerUpgradesSo);
        SceneManager.LoadScene("UpgradeMenu");
    }
    
}
