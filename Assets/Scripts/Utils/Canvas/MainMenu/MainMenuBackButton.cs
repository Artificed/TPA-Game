using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackButton : ButtonHandler
{
    [SerializeField] private GameObject alertModal;
    [SerializeField] private AudioClip closeModalAudio;

    private void Start()
    {
        button.onClick.AddListener(HandleCloseModal);
    }
    
    public void HandleCloseModal()
    {
        currentClickSound = closeModalAudio;
        alertModal.SetActive(false);
    }
}
