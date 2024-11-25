using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/ZhenCounterEventChannel", fileName = "ZhenCounterEventChannel")]
public class ZhenCounterEventChannel : ScriptableObject
{
    public UnityAction<int> OnZhenChanged;

    public void RaiseEvent(int level)
    {
        OnZhenChanged.Invoke(level);
    }
}
