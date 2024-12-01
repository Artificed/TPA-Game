using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UpgradeMenuExitButton : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private PlayerUpgradesSO playerUpgradesSo;

    public void HandleExit()
    {
        SaveSystem.SaveGameData(playerDataSo, playerUpgradesSo);
        SceneManager.LoadScene("MainMenu");
    }
}
