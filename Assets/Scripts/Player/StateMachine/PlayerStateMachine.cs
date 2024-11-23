using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private EnemyAlertEventChannel enemyAlertEventChannel;
    [SerializeField] private PlayerTurnEventChannel playerTurnEventChannel;
    public static PlayerStateMachine Instance { get; private set; }
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform unit;
    [SerializeField] private Animator animator;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AStar pathFinder;

    private int _isMovingHash;
    private List<Tile> path = new List<Tile>();
    private PlayerBaseState _currentState;
    private PlayerStateFactory _stateFactory;
    private bool _cancellingPath;
    private bool _isCurrentlyMoving;

    private bool _withinEnemyRange;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }
    }
    
    private void OnEnable()
    {
        if (enemyAlertEventChannel != null)
        {
            enemyAlertEventChannel.enemyAlertEvent.AddListener(HandleEnemyAlert);
        }
    }

    private void OnDisable()
    {
        if (enemyAlertEventChannel != null)
        {
            enemyAlertEventChannel.enemyAlertEvent.RemoveListener(HandleEnemyAlert);
        }
    }
    
    private void HandleEnemyAlert(bool isInRange)
    {
        CancellingPath = isInRange;
        _withinEnemyRange = isInRange;
    }
    
    void Start()
    {
        _isMovingHash = Animator.StringToHash("isMoving");
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<AStar>();
        
        _stateFactory = new PlayerStateFactory(this);
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

    public PlayerBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
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
    

    public int IsMovingHash
    {
        get => _isMovingHash;
        set => _isMovingHash = value;
    }

    public List<Tile> Path
    {
        get => path;
        set => path = value;
    }

    public bool CancellingPath
    {
        get => _cancellingPath;
        set => _cancellingPath = value;
    }

    public bool IsCurrentlyMoving
    {
        get => _isCurrentlyMoving;
        set => _isCurrentlyMoving = value;
    }

    public bool WithinEnemyRange
    {
        get => _withinEnemyRange;
        set => _withinEnemyRange = value;
    }

    public PlayerStateFactory StateFactory
    {
        get => _stateFactory;
        set => _stateFactory = value;
    }

    public PlayerTurnEventChannel PlayerTurnEventChannel
    {
        get => playerTurnEventChannel;
        set => playerTurnEventChannel = value;
    }
}
