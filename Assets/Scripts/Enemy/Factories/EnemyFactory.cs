using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFactory: MonoBehaviour
{
    [SerializeField] private EnemyDataSO commonEnemySO;
    [SerializeField] private EnemyDataSO mediumEnemySO;
    [SerializeField] private EnemyDataSO eliteEnemySO;

    public void CreateCommonEnemy()
    {
        
    }
    
    public void CreateMediumEnemy()
    {
        
    }
    
    public void CreateEliteEnemy()
    {
        
    }

    private void CreateEnemy(EnemyDataSO enemyData, Vector2Int coords)
    {
        GameObject newEnemy = Instantiate(enemyData.GameObject());
    }

}
