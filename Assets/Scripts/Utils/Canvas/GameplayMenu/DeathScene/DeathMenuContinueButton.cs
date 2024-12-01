using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuContinueButton : ButtonHandler
{
    void Start()
    {
        button.onClick.AddListener(HandleContinueClick);
    }

    private void HandleContinueClick()
    {
        audioSource.PlayOneShot(normalClickSound);
        SceneManager.LoadScene("UpgradeMenu");
    }
}
