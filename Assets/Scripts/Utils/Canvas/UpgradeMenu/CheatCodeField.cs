using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CheatCodeField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI zhenCounter;
    
    [SerializeField] private TMP_InputField cheatCodeInputField;
    [SerializeField] private GameObject placeHolderText;
    
    [SerializeField] private PlayerDataSO playerDataSo;
    
    [SerializeField] private AudioClip cheatSound;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private PlayerFloorCountEventChannel playerFloorCountEventChannel;
    
    void Update()
    {
        HandlePlaceholder();
        HandleCheatCode();
    }

    private void HandlePlaceholder()
    {
        if (cheatCodeInputField.text.Trim().Length > 0)
        {
            placeHolderText.SetActive(false);
        }
    }

    private void HandleCheatCode()
    {
        if (cheatCodeInputField.text.Equals("hesoyam"))
        {
            audioSource.PlayOneShot(cheatSound);
            playerDataSo.IncreaseExp(10000);
            cheatCodeInputField.text = "";
        } 
        else if (cheatCodeInputField.text.Equals("tpagamegampang"))
        {
            audioSource.PlayOneShot(cheatSound);
            playerDataSo.zhen += 10000;
            zhenCounter.text = playerDataSo.zhen.ToString();
            cheatCodeInputField.text = "";
        } 
        else if (cheatCodeInputField.text.Equals("opensesame"))
        {
            audioSource.PlayOneShot(cheatSound);
            playerDataSo.floor = 100;
            cheatCodeInputField.text = "";
            playerFloorCountEventChannel.RaiseEvent();
        }
    }
}
