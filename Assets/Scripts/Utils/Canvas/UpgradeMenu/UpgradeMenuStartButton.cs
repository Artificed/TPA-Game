using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeMenuStartButton : ButtonHandler
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private PlayerDataSO playerDataSo;

    private void Start()
    {
        button.onClick.AddListener(HandleGameStart);
    }

    private void HandleGameStart()
    {
        int selectedFloor = dropdown.value;
        playerDataSo.selectedFloor = selectedFloor;
        currentClickSound = normalClickSound;
        SceneManager.LoadScene("GameScene");
    }
}
