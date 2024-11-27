using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EnemyLeftEventChannel", fileName = "EnemyLeftEventChannel")]
public class EnemyLeftEventChannel : ScriptableObject
{
    public UnityAction<int> OnEnemyLeftChanged;

    public void RaiseEvent(int currentEnemyCount)
    {
        OnEnemyLeftChanged.Invoke(currentEnemyCount);
    }
}
