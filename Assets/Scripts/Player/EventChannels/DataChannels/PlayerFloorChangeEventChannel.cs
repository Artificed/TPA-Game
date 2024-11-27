using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/PlayerFloorChangeEventChannel", fileName = "PlayerFloorChangeEventChannel")]
public class PlayerFloorChangeEventChannel : ScriptableObject
{
    public UnityAction<int> OnFloorChanged;

    public void RaiseEvent(int floor)
    {
        OnFloorChanged.Invoke(floor);
    }
}
