using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerFloorCountEventChannel", menuName = "EventChannels/PlayerFloorCountEventChannel")]
public class PlayerFloorCountEventChannel : ScriptableObject
{
    public UnityAction FloorChangedEvent;

    public void RaiseEvent()
    {
        FloorChangedEvent.Invoke();
    }
}
