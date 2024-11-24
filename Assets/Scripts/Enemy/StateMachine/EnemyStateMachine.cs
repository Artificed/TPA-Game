using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform unit;
    [SerializeField] private Animator animator;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AStar pathFinder;
    
    [SerializeField] private EnemyAlertEventChannel enemyAlertEventChannel;
    [SerializeField] private EnemyAlertHelper alertHelper;
    [SerializeField] private EnemyActionCompleteEventChannel enemyActionCompleteEventChannel;
    
    private Transform _playerTransform;
    private const float alertRange = 5.0f;
    
    private EnemyStateFactory _stateFactory;
    private EnemyBaseState _currentState;
    
    private List<Tile> path = new List<Tile>();
    
    private int _isAlertHash;
    private int _isMovingHash;
    private int _isAttackingHash;
    
    private int recalculationCount = 0;
    private const int maxRecalculationAttempts = 5;
    
    void Start()
    {
        _playerTransform = PlayerStateMachine.Instance.transform;
        
        _isAlertHash = Animator.StringToHash("isAlert");
        _isMovingHash = Animator.StringToHash("isMoving");
        _isAttackingHash = Animator.StringToHash("isAttacking");
                    
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<AStar>();

        _stateFactory = new EnemyStateFactory(this);
        _currentState = _stateFactory.CreateIdle();
        _currentState.EnterState();
    }
    
    void Update()
    {
        _currentState.UpdateState();
    }
    
    public void ClearPath()
    {
        StopAllCoroutines();
        path.Clear();
    }
    
    public void SetNewDestination(Vector2Int startCords, Vector2Int targetCords)
    {
        if (!gridManager.Grid.ContainsKey(startCords) || !gridManager.Grid.ContainsKey(targetCords))
        {
            ClearPath(); 
            return;
        }

        if (gridManager.Grid[startCords].Blocked || gridManager.Grid[targetCords].Blocked)
        {
            ClearPath(); 
            return;
        }
        
        pathFinder.SetNewDestination(startCords, targetCords);
        RecalculatePath();
    }

    private bool IsPathBlockedByEnemy(List<Tile> path)
    {
        List<EnemyStateMachine> enemies = TurnManager.Instance.Enemies;
        List<Vector2Int> enemyPositions = new List<Vector2Int>();
        
        foreach (EnemyStateMachine enemy in enemies)
        {
            if (enemy != this)
            {
                Vector2Int enemyCoords = new Vector2Int(
                    Mathf.RoundToInt(enemy.Unit.position.x / gridManager.UnityGridSize),
                    Mathf.RoundToInt(enemy.Unit.position.z / gridManager.UnityGridSize)
                );
                enemyPositions.Add(enemyCoords);
            }
        }
        
        foreach (var tile in path)
        {
            Vector2Int tileCoords = new Vector2Int(
                Mathf.RoundToInt(tile.transform.position.x / gridManager.UnityGridSize),
                Mathf.RoundToInt(tile.transform.position.z / gridManager.UnityGridSize)
            );

            if (enemyPositions.Contains(tileCoords))
            {
                return true;
            }
        }

        return false;
    }

    private Vector2Int GetValidTile(Vector2Int dest)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector2Int candidateCoord = new Vector2Int(dest.x + x, dest.y + y);
                if (!gridManager.Grid.ContainsKey(candidateCoord) || gridManager.Grid[candidateCoord].Blocked)
                {
                    continue;
                }
                return candidateCoord;
            }
        }
        return dest;
    }
    
    private void RecalculatePath()
    {
        if (recalculationCount >= maxRecalculationAttempts)
        {
            Debug.LogError("Max path recalculation attempts reached.");
            recalculationCount = 0; 
            return;
        }
        
        StopAllCoroutines();
        path.Clear();

        List<Tile> newPath = pathFinder.GetNewPath();
        if (IsPathWalkable(newPath) && !IsPathBlockedByEnemy(newPath))
        {
            path = newPath;
            recalculationCount = 0; 
        }
        else
        {
            Debug.Log("Blocked by enemy, recalculating");
            Vector2Int newDest = GetValidTile(newPath.Last().coords);
            if (!newDest.Equals(newPath.Last().coords)) 
            {
                recalculationCount++; 
                SetNewDestination(newPath.First().coords, newDest);
            }
            else
            {
                Debug.LogError("No alternative path found. Consider a fallback strategy.");
                recalculationCount = 0; 
            }
        }
    }
    
    public bool IsPathWalkable(List<Tile> path)
    {
        foreach (Tile node in path)
        {
            if (node.Blocked)
            {
                return false;
            }
        }
        return true;
    }

    public EnemyBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    public float GetDistanceFromPlayer()
    {
        Vector2Int playerCoords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / gridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / gridManager.UnityGridSize)
        );

        Vector2Int enemyCoords = new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize),
            Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize)
        );

        float distance = Vector2.Distance(playerCoords, enemyCoords);
        
        return distance;
    }

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public Transform Unit
    {
        get => unit;
        set => unit = value;
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public GridManager GridManager
    {
        get => gridManager;
        set => gridManager = value;
    }

    public AStar PathFinder
    {
        get => pathFinder;
        set => pathFinder = value;
    }

    public Transform PlayerTransform
    {
        get => _playerTransform;
        set => _playerTransform = value;
    }

    public EnemyStateFactory StateFactory
    {
        get => _stateFactory;
        set => _stateFactory = value;
    }

    public int IsAlertHash
    {
        get => _isAlertHash;
        set => _isAlertHash = value;
    }

    public List<Tile> Path
    {
        get => path;
        set => path = value;
    }
    
    public float AlertRange
    {
        get => alertRange;
    }

    public int IsMovingHash
    {
        get => _isMovingHash;
        set => _isMovingHash = value;
    }

    public int IsAttackingHash
    {
        get => _isAttackingHash;
        set => _isAttackingHash = value;
    }

    public EnemyActionCompleteEventChannel EnemyActionCompleteEventChannel
    {
        get => enemyActionCompleteEventChannel;
        set => enemyActionCompleteEventChannel = value;
    }
}
