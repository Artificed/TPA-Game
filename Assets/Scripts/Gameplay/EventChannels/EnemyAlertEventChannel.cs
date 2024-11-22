using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyAlertEventChannel", menuName = "EventChannels/EnemyAlert")]
public class EnemyAlertEventChannel : ScriptableObject
{
    public UnityEvent enemyAlertEvent;
    private void OnEnable()
    {
        enemyAlertEvent ??= new UnityEvent();
    }

    public void RaiseEvent()
    {
        enemyAlertEvent.Invoke();
    }
}