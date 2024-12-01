using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNewGameButton : ButtonHandler
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;
    [SerializeField] private List<UpgradeDataSO> upgradeDataSos;
    
    [SerializeField] private GameObject alertModal;

    [SerializeField] private AudioClip openModalAudio;
    
    private SaveData _saveData;
    private bool _saveExists;
    
    private void Start()
    {
        GetSaveData();
    }
    
    private void GetSaveData()
    {
        _saveExists = SaveSystem.SaveDataExists();
        if (_saveExists)
        {
            button.onClick.AddListener(OpenModal);
        }
        else
        {
            button.onClick.AddListener(HandleNewGame);
        }
    }

    public void HandleNewGame()
    {
        ResetData();
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

    public void OpenModal()
    {
        currentClickSound = openModalAudio;
        alertModal.SetActive(true);
    }
}
