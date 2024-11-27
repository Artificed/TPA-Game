using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/PlayerLevelEventChannel", fileName = "PlayerLevelEventChannel")]
public class PlayerLevelEventChannel : ScriptableObject
{
    public UnityAction<int> OnLevelChanged;

    public void RaiseEvent(int level)
    {
        OnLevelChanged.Invoke(level);
    }
}
