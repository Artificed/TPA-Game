using System;
using System.Collections;
using System.Collections.Generic;
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

    private Transform _playerTransform;
    private const float alertRange = 5.0f;
    
    private EnemyStateFactory _stateFactory;
    private EnemyBaseState _currentState;
    
    private int _isAlertHash;
    private List<Tile> path = new List<Tile>();
    private int _isMovingHash;
    
    void Start()
    {
        _playerTransform = PlayerStateMachine.Instance.transform;
        
        _isAlertHash = Animator.StringToHash("isAlert");
        _isMovingHash = Animator.StringToHash("isMoving");
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
    
    private void RecalculatePath()
    {
        StopAllCoroutines();
        path.Clear();

        List<Tile> newPath = pathFinder.GetNewPath();
        if (IsPathWalkable(newPath))
        {
            path = newPath;
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
}
