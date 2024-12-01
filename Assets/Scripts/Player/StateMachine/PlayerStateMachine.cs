using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private EnemyAlertEventChannel enemyAlertEventChannel;
    [SerializeField] private PlayerTurnEventChannel playerTurnEventChannel;
    [SerializeField] private CameraShakeEventChannel cameraShakeEventChannel;
    [SerializeField] private PlayerActiveSkillEventChannel playerActiveSkillEventChannel;
    [SerializeField] private PlayerBuffSkillEventChannel playerBuffSkillEventChannel;
    [SerializeField] private BuffDisplayEventChannel buffDisplayEventChannel;
    [SerializeField] private PlayerDeathEventChannel playerDeathEventChannel;
    [SerializeField] private GameObject buffAura;
    
    public static PlayerStateMachine Instance { get; private set; }
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform unit;
    [SerializeField] private Animator animator;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AStar pathFinder;

    private int _isMovingHash;
    private int _isAttackingHash1;
    private int _isAttackingHash2;
    private int _isAttackingHash3;
    private int _isHitHash;
    private int _isDeadHash;
    private int _isBuffHash;
    
    [SerializeField] private int attackAnimationsCount = 3;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject statusTextPrefab;
    
    private List<Tile> path = new List<Tile>();
    private PlayerBaseState _currentState;
    private PlayerStateFactory _stateFactory;
    private bool _cancellingPath;
    private bool _isCurrentlyMoving;

    private bool _withinEnemyRange;
    
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }
        
        _isMovingHash = Animator.StringToHash("isMoving");
        _isAttackingHash1 = Animator.StringToHash("isAttacking1");
        _isAttackingHash2 = Animator.StringToHash("isAttacking2");
        _isAttackingHash3 = Animator.StringToHash("isAttacking3");
        _isHitHash = Animator.StringToHash("isHit");
        _isDeadHash = Animator.StringToHash("isDead");
        _isBuffHash = Animator.StringToHash("isBuffing");
        
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<AStar>();
        
        _stateFactory = new PlayerStateFactory(this);
        _currentState = _stateFactory.CreateIdle();
        _currentState.EnterState();
    }

    void Update()
    {
        // Debug.Log(CurrentState);
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
        SoundFXManager.Instance.PlayTakeDamageClip(transform);
        TurnManager.Instance.CurrentEnemyTarget.Animator.SetTrigger(TurnManager.Instance.CurrentEnemyTarget.IsHitHash);
    }
    
    public void showDamageText(int damage, bool isCritical = false)
    {
        var canvas = GetComponentInChildren<Canvas>().transform;
        var damageText = Instantiate(statusTextPrefab, canvas);
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

    public void showLevelUpText()
    { 
        var canvas = GetComponentInChildren<Canvas>().transform;
        var levelUpText = Instantiate(statusTextPrefab, canvas);
        var textMesh = levelUpText.GetComponent<TextMeshPro>();
        textMesh.text = "Level Up!";
        levelUpText.transform.localPosition = new Vector3(0, 50.0f, 0); 
        textMesh.color = new Color(1f, 0.7f, 0f);
        textMesh.fontSize = 14;
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

    public int IsAttackingHash1
    {
        get => _isAttackingHash1;
        set => _isAttackingHash1 = value;
    }
    
    public int IsAttackingHash2
    {
        get => _isAttackingHash2;
        set => _isAttackingHash2 = value;
    }
    
    public int IsAttackingHash3
    {
        get => _isAttackingHash3;
        set => _isAttackingHash3 = value;
    }
    
    public int AttackAnimationsCount
    {
        get => attackAnimationsCount;
        set => attackAnimationsCount = value;
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

    public CameraShakeEventChannel CameraShakeEventChannel
    {
        get => cameraShakeEventChannel;
        set => cameraShakeEventChannel = value;
    }

    public int IsBuffHash
    {
        get => _isBuffHash;
        set => _isBuffHash = value;
    }

    public PlayerActiveSkillEventChannel PlayerActiveSkillEventChannel
    {
        get => playerActiveSkillEventChannel;
        set => playerActiveSkillEventChannel = value;
    }

    public PlayerBuffSkillEventChannel PlayerBuffSkillEventChannel
    {
        get => playerBuffSkillEventChannel;
        set => playerBuffSkillEventChannel = value;
    }

    public BuffDisplayEventChannel BuffDisplayEventChannel
    {
        get => buffDisplayEventChannel;
        set => buffDisplayEventChannel = value;
    }

    public void ActivateParticles()
    {
        buffAura.SetActive(true);
    }

    public void DeactivateParticles()
    {
        buffAura.SetActive(false);
    }

    public PlayerDeathEventChannel PlayerDeathEventChannel
    {
        get => playerDeathEventChannel;
        set => playerDeathEventChannel = value;
    }
}
