using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloorClearedEventChannel", menuName = "EventChannels/FloorClearedEventChannel")]
public class FloorClearedEventChannel : ScriptableObject
{
    public UnityAction FloorClearedEvent;
    
    public void RaiseEvent()
    {
        FloorClearedEvent.Invoke();
    }
}
