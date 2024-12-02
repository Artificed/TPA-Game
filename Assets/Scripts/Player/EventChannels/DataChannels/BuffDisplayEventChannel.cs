using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "EventChannels/BuffDisplayEventChannel", fileName = "BuffDisplayEventChannel")]
public class BuffDisplayEventChannel : ScriptableObject
{
    public UnityAction OnBuffRefreshed;
    
    public void RefreshBuff()
    {
        OnBuffRefreshed?.Invoke();
    }
}
