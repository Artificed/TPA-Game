using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class EnemyFactory: MonoBehaviour
{    
    public static EnemyFactory Instance { get; private set; }
    
    [SerializeField] private EnemyDataSO commonEnemySO;
    [SerializeField] private EnemyDataSO mediumEnemySO;
    [SerializeField] private EnemyDataSO eliteEnemySO;

    [SerializeField] private EnemyNamesSO enemyNamesSO;

    private Random _random = new Random();
    
    public GameObject CreateCommonEnemy(Vector2Int coords, int level)
    {
        return CreateEnemy(commonEnemySO, coords, level);
    }
    
    public GameObject CreateMediumEnemy(Vector2Int coords, int level)
    {
        return CreateEnemy(mediumEnemySO, coords, level);
    }

    public GameObject CreateEliteEnemy(Vector2Int coords, int level)
    {
        return CreateEnemy(eliteEnemySO, coords, level);
    }

    private GameObject CreateEnemy(EnemyDataSO enemyData, Vector2Int coords, int level)
    {
        GameObject newEnemy = Instantiate(enemyData.enemyPrefab, new Vector3(coords.x, 0.1f, coords.y), Quaternion.identity);

        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.Initialize(enemyData, GetName(), level);
        }

        return newEnemy;
    }

    private string GetName()    
    {
        int total = enemyNamesSO.names.Count;
        return enemyNamesSO.names[_random.Next(total)];
    }
    
}
