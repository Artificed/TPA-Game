using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/PlayerExpEventChannel", fileName = "PlayerExpEventChannel")]
public class PlayerExpEventChannel : ScriptableObject
{
    public UnityAction<int, int> OnExpChanged;

    public void RaiseEvent(int currentExp, int expCap)
    {
        OnExpChanged.Invoke(currentExp, expCap);
    }
}