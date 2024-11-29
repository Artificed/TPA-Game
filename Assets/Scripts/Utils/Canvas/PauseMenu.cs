using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;

    [SerializeField] private GameObject pauseModal;
    
    [SerializeField] private AudioClip openModalSound;
    [SerializeField] private AudioClip closeModalSound;
    
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                HandleResume();
            }
            else
            {
                HandlePause();
            }
        }
    }

    private void HandlePause()
    {
        audioSource.PlayOneShot(openModalSound);
        pauseModal.SetActive(true);
        
        Time.timeScale = 0;
        GamePaused = true;
    }
    
    public void HandleResume()
    {
        audioSource.PlayOneShot(closeModalSound);
        pauseModal.SetActive(false);
        
        Time.timeScale = 1;
        GamePaused = false;
    }

    public void HandleUpgrade()
    {
        
    }

    public void HandleExit()
    {
        
    }
}
