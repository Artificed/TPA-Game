using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorClearedMenu : MonoBehaviour
{
    [SerializeField] private FloorClearedEventChannel floorClearedEventChannel;
    [SerializeField] private GameObject floorClearedCanvas;
    
    public void HandleFloorChange()
    {
        floorClearedCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        floorClearedEventChannel.FloorClearedEvent += HandleFloorChange;
    }

    private void OnDisable()
    {
        floorClearedEventChannel.FloorClearedEvent -= HandleFloorChange;
    }
}
