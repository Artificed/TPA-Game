using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerTurnEventChannel", menuName = "EventChannels/PlayerTurn")]
public class PlayerTurnEventChannel : ScriptableObject
{
    public UnityEvent playerTurnEvent;
    
    private void OnEnable()
    {
        playerTurnEvent ??= new UnityEvent();
    }
    
    public void RaiseEvent()
    {
        playerTurnEvent?.Invoke();
    }
}
