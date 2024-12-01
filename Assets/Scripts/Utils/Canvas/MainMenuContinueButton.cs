using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuContinueButton : ButtonHandler
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;

    [SerializeField] private GameObject darkPanel;
    
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
            darkPanel.SetActive(false);
            _saveData = SaveSystem.LoadGameData();
            button.onClick.AddListener(HandleContinue);
        }
        else
        {
            darkPanel.SetActive(true);
            button.onClick.RemoveAllListeners();
            button.enabled = false;
        }
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_saveExists)
        {
            button.image.sprite = buttonHovered;
            audioSource.PlayOneShot(buttonHoverSound);
        }
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (_saveExists)
        {
            audioSource.PlayOneShot(currentClickSound);
            currentClickSound = normalClickSound;
        }
    }
    
    public void HandleContinue()
    {
        if (_saveData != null)
        {
            _saveData.TransferData(playerDataSo, playerUpgradesSo);
            SceneManager.LoadScene("UpgradeMenu");
        }
    }
}
