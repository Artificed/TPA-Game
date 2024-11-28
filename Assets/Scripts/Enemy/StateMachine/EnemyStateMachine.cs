using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform unit;
    [SerializeField] private Animator animator;
    [SerializeField] private AStar pathFinder;
    [SerializeField] private EnemyAlertEventChannel enemyAlertEventChannel;
    [SerializeField] private EnemyActionCompleteEventChannel enemyActionCompleteEventChannel;
    [SerializeField] private CameraShakeEventChannel cameraShakeEventChannel;
    
    private GridManager _gridManager = GridManager.Instance;
    
    private Transform _playerTransform;
    private const float alertRange = 5.0f;
    
    private EnemyStateFactory _stateFactory;
    private EnemyBaseState _currentState;
    
    private List<Tile> _path = new List<Tile>();
    
    private int _isAlertHash;
    private int _isMovingHash;
    private int _isAttackingHash;
    private int _isHitHash;
    private int _isDeadHash;

    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject exclamationText;
    [SerializeField] private GameObject questionText;
    [SerializeField] private GameObject damageTextPrefab;
    
    private int _recalculationCount = 0;
    private const int MaxRecalculationAttempts = 10;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        
        _playerTransform = PlayerStateMachine.Instance.transform;
        
        _isAlertHash = Animator.StringToHash("isAlert");
        _isMovingHash = Animator.StringToHash("isMoving");
        _isAttackingHash = Animator.StringToHash("isAttacking");
        _isHitHash = Animator.StringToHash("isHit");
        _isDeadHash = Animator.StringToHash("isDead");
                    
        _gridManager = FindObjectOfType<GridManager>();
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
        _path.Clear();
    }
    
    public void SetNewDestination(Vector2Int startCords, Vector2Int targetCords)
    {
        if (!_gridManager.Grid.ContainsKey(startCords) || !_gridManager.Grid.ContainsKey(targetCords))
        {
            ClearPath(); 
            return;
        }

        if (_gridManager.Grid[startCords].Blocked || _gridManager.Grid[targetCords].Blocked)
        {
            ClearPath(); 
            return;
        }
        
        pathFinder.SetNewDestination(startCords, targetCords);
        RecalculatePath();
    }

    private bool IsPathBlockedByEnemy(List<Tile> path)
    {
        List<Enemy> enemies = TurnManager.Instance.ActiveEnemies;
        List<Vector2Int> enemyPositions = new List<Vector2Int>();
        
        foreach (Enemy enemy in enemies)
        {
            if (enemy == this.Enemy) continue;
            Vector2Int enemyCoords = new Vector2Int(
                Mathf.RoundToInt(enemy.EnemyStateMachine.Unit.position.x / _gridManager.UnityGridSize),
                Mathf.RoundToInt(enemy.EnemyStateMachine.Unit.position.z / _gridManager.UnityGridSize)
            );
            enemyPositions.Add(enemyCoords);
        }
        
        foreach (var tile in path)
        {
            Vector2Int tileCoords = new Vector2Int(
                Mathf.RoundToInt(tile.transform.position.x / _gridManager.UnityGridSize),
                Mathf.RoundToInt(tile.transform.position.z / _gridManager.UnityGridSize)
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
                if (!_gridManager.Grid.ContainsKey(candidateCoord) || _gridManager.Grid[candidateCoord].Blocked)
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
        if (_recalculationCount >= MaxRecalculationAttempts)
        {
            Debug.LogError("Max path recalculation attempts reached.");
            _recalculationCount = 0; 
            return;
        }
        
        StopAllCoroutines();
        _path.Clear();

        List<Tile> newPath = pathFinder.GetNewPath();
        if (IsPathWalkable(newPath) && !IsPathBlockedByEnemy(newPath))
        {
            _path = newPath;
            _recalculationCount = 0; 
        }
        else
        {
            Debug.Log("Blocked by enemy, recalculating");
            Vector2Int newDest = GetValidTile(newPath.Last().coords);
            if (!newDest.Equals(newPath.Last().coords)) 
            {
                _recalculationCount++; 
                SetNewDestination(newPath.First().coords, newDest);
            }
            else
            {
                Debug.LogError("No alternative path found");
                _currentState.SwitchState(_stateFactory.CreateAggro());
                _recalculationCount = 0; 
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
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / _gridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / _gridManager.UnityGridSize)
        );

        Vector2Int enemyCoords = new Vector2Int(
            Mathf.RoundToInt(transform.position.x / _gridManager.UnityGridSize),
            Mathf.RoundToInt(transform.position.z / _gridManager.UnityGridSize)
        );

        float distance = Vector2.Distance(playerCoords, enemyCoords);
        
        return distance;
    }

    public void ShowSword()
    {
        sword.SetActive(true);
    }

    public void HideSword()
    {
        sword.SetActive(false);
    }

    public void PlayerKnockBack()
    {
        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.IsHitHash);
    }

    public void showDamageText(int damage, bool isCritical = false)
    {
        var canvas = GetComponentInChildren<Canvas>().transform;
        var damageText = Instantiate(damageTextPrefab, canvas);
        var textMesh = damageText.GetComponent<TextMeshPro>();

        textMesh.text = damage.ToString();
        damageText.transform.localPosition = new Vector3(0, 50.0f, 0); 
        
        if (isCritical)
        {
            textMesh.color = new Color(1f, 0.3f, 0f); 
            textMesh.fontSize = 20; 
        }
        else
        {
            textMesh.color = Color.white; 
            textMesh.fontSize = 12; 
        }
    }

    public int IsHitHash
    {
        get => _isHitHash;
        set => _isHitHash = value;
    }

    public int IsDeadHash
    {
        get => _isDeadHash;
        set => _isDeadHash = value;
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
        get => _gridManager;
        set => _gridManager = value;
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
        get => _path;
        set => _path = value;
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

    public GameObject ExclamationText
    {
        get => exclamationText;
        set => exclamationText = value;
    }

    public GameObject QuestionText
    {
        get => questionText;
        set => questionText = value;
    }

    public Enemy Enemy
    {
        get => enemy;
        set => enemy = value;
    }

    public CameraShakeEventChannel CameraShakeEventChannel => cameraShakeEventChannel;
}
