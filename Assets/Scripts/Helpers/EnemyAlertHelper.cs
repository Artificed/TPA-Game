using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertHelper : MonoBehaviour
{
    public static EnemyAlertHelper Instance { get; private set; }
    
    [SerializeField] private float alertRange = 5f;
    [SerializeField] private EnemyAlertEventChannel alertEventChannel;

    private List<EnemyStateMachine> enemies;
    private Transform playerTransform;
    
    private void Start()
    {
        playerTransform = PlayerStateMachine.Instance.transform;
        InvokeRepeating(nameof(CheckEnemyDistances), 0f, 0.5f);
    }

    private void CheckEnemyDistances()
    {
        enemies = new List<EnemyStateMachine>(FindObjectsOfType<EnemyStateMachine>());
        bool inRange = false;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance <= alertRange)
            {
                inRange = true;
                break;
            }
        }
        
        alertEventChannel.RaiseEvent(inRange);
    }
}