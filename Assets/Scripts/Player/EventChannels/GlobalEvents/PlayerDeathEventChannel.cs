using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerDeathEventChannel", menuName = "EventChannels/PlayerDeathEventChannel")]
public class PlayerDeathEventChannel : ScriptableObject
{
    public UnityAction PlayerDeathEvent;
    
    public void RaiseEvent()
    {
        PlayerDeathEvent.Invoke();
    }
}
