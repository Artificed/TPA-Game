using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyActionCompleteEventChannel", menuName = "EventChannels/EnemyActionComplete")]
public class EnemyActionCompleteEventChannel : ScriptableObject
{
    public UnityEvent OnActionComplete;

    public void RaiseEvent()
    {
        OnActionComplete?.Invoke();
    }
}
