using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyAlertEventChannel", menuName = "EventChannels/EnemyAlert")]
public class EnemyAlertEventChannel : ScriptableObject
{
    public UnityEvent<bool> enemyAlertEvent;

    private void OnEnable()
    {
        enemyAlertEvent ??= new UnityEvent<bool>();
    }
    
    public void RaiseEvent(bool isInRange)
    {
        enemyAlertEvent?.Invoke(isInRange);
    }
}