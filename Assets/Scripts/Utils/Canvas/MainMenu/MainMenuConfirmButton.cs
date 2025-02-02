using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuConfirmButton : ButtonHandler
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;
    [SerializeField] private List<UpgradeDataSO> upgradeDataSos;
    
    [SerializeField] private GameObject alertModal;

    public void Start()
    {
        button.onClick.AddListener(HandleNewGame);
    }
    
    public void HandleNewGame()
    {
        ResetData();
        SaveSystem.DeleteFile();
        currentClickSound = normalClickSound;
        SceneManager.LoadScene("UpgradeMenu");
    }
    
    public void ResetData()
    {
        playerDataSo.Reset();
        playerUpgradesSo.Reset();
        foreach (var upgradeDataSo in upgradeDataSos)
        {
            upgradeDataSo.Reset();
        }
    }
}
