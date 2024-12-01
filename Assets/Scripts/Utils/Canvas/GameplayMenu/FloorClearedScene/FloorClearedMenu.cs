using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorClearedMenu : MonoBehaviour
{
    [SerializeField] private FloorClearedEventChannel floorClearedEventChannel;
    [SerializeField] private GameObject floorClearedCanvas;

    [SerializeField] private PlayerFloorChangeEventChannel playerFloorChangeEventChannel; 
    
    private bool _floorChanged = false;
    
    public void HandleFloorChange()
    {
        if(_floorChanged) return;
        
        Player.Instance.Floor++;
        Player.Instance.SelectedFloor++;
        floorClearedCanvas.SetActive(true);
        Time.timeScale = 0;
        _floorChanged = true;
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