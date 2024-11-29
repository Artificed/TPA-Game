using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorClearedMenu : MonoBehaviour
{
    [SerializeField] private FloorClearedEventChannel floorClearedEventChannel;
    [SerializeField] private GameObject floorClearedCanvas;

    [SerializeField] private PlayerFloorChangeEventChannel playerFloorChangeEventChannel; 
    
    public void HandleFloorChange()
    {
        Player.Instance.Floor++;
        Player.Instance.SavePlayerData();
        floorClearedCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        floorClearedEventChannel.FloorClearedEvent += HandleFloorChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        floorClearedEventChannel.FloorClearedEvent -= HandleFloorChange;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
    }

    public void HandleButtonPress()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}