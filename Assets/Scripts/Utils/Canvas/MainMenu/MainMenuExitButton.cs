using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuExitButton : ButtonHandler
{
    void Start()
    {
        button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
