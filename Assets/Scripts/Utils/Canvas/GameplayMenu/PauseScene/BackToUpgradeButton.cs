using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToUpgradeButton : ButtonHandler
{
    void Start()
    {
        button.onClick.AddListener(HandleBackToUpgradeClick);
    }

    private void HandleBackToUpgradeClick()
    {
        audioSource.PlayOneShot(normalClickSound);
        SceneManager.LoadScene("UpgradeMenu");
    }
}
